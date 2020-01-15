using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

using UnityEngine.UI;

using System.IO;
using System.Text;
using System;



public class GoogleManager : MonoBehaviour
{
    private const string FILE_NAME = "saveFile";
    private Action<bool> SignInCallBack;

    public StageData stageData;
    //public GameObject LoadingObj;

    public int isReadyToLogIn;

    public Text text;

    public void Awake()
    {
        // "저장된 게임" 기능 사용 
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignInCallBack = (bool success) =>
        {
            if (success)
            {
                //Debug용 
                //STATUS.text = "LOGIN";
                //username.text = Social.localUser.userName;

            }
            else
            {
                //Debug용
                //STATUS.text = "LOGIN FAIL2";
            }
        };
    }

    // 로그인 
    public IEnumerator LogIn()
    {
        Debug.Log(isReadyToLogIn);
        // 로그인이 되지 않았으면 로그인하기 -> 버튼을 눌렀을 때 로그인 하는 걸로 바꿈 
        while (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(SignInCallBack);
            yield return new WaitForSeconds(1f);
        }
        //text.text = "LOG IN";
        LoadFromCloud();
        
    }

    //로그아웃
    public void LogOut()
    {
        if (!Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
        }
    }

    public void Open_SavedData(string file, bool Save) 
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        //저장 루틴 
        if (Save)
        {
            savedGameClient.OpenWithAutomaticConflictResolution(file, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToSave);
        }

        //로딩 루틴 
        else
        {
            savedGameClient.OpenWithAutomaticConflictResolution(file, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OpenToRead);
        }
    }

    #region Save

    public void SaveToCloud()
    {
        if (!Social.localUser.authenticated)
        {
            //STATUS.text = "SaveToCloud fail";
            LogIn();
            return;
        }
        Debug.Log("LOG IN To Save Data");
        Open_SavedData(FILE_NAME, true);
    }

    private void OpenToSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            //이곳에 맵 클리어 여부 데이터를 바이트로 변환한 것을 넣으면 됨 
            Debug.Log("Saved Game Request Status :: Success (OPEN TO SAVE)");
            string DataString = JsonUtility.ToJson(stageData);
            byte[] DataByte = Encoding.UTF8.GetBytes(DataString);

            SaveGame(game, DataByte, DateTime.Now.TimeOfDay);
        }
        else
        {
            //파일열기 실패
            // STATUS.text = "파일열기 실패1";
            Debug.Log("Saved Game Request Status :: Fail (OPEN TO SAVE)");
        }
    }

    private void SaveGame(ISavedGameMetadata game, byte[] data, TimeSpan totalPlayTime)
    {
        //STATUS.text = "SaveGame Phase";
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder.WithUpdatedPlayedTime(totalPlayTime).WithUpdatedDescription("Save game at" + DateTime.Now);

        SavedGameMetadataUpdate metaDataUpdate = builder.Build();
        savedGameClient.CommitUpdate(game, metaDataUpdate, data, SavedWritten);
    }

    private void SavedWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            //데이터 저장 완료
            // STATUS.text = "데이터 저장 완료"; 
            Debug.Log("Save Success");

        }
        else
        {
            Debug.Log("Save Fail");
            //데이터 저장 실패 
           // STATUS.text = "데이터 저장 실패";
        }
    }

    #endregion




    #region Load

    //클라우드에 저장한 데이터 불러오기 
    public void LoadFromCloud()
    {
        if (!Social.localUser.authenticated)
        {
            LogIn();
            return;
        }
        Open_SavedData("saveFile", false);
    }

    private void OpenToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        //text.text = "OPEN TO READ";
        if(status == SavedGameRequestStatus.Success)
        {
           //클라우드 파일 열기 성공 
            LoadGame(game);
        }
        else
        {
            //클라우드 파일 열기 실패 
        }
    }

    private void LoadGame(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, DataRead);
    }

    private void DataRead(SavedGameRequestStatus status, byte[] data)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            if(data.Length == 0)
            {
                Debug.Log("LoadFail");
                //text.text = "LOAD FAIL";
                TextAsset jsonString = Resources.Load("saveFile") as TextAsset;
                string LocalDataString = jsonString.text;
                stageData = JsonUtility.FromJson<StageData>(LocalDataString);
                //SaveToCloud();
                //text.text = LocalDataString;
            }
            else
            {
                Debug.Log("Load");
                //text.text = "LOAD SUCCESS";
                string DataString = Encoding.UTF8.GetString(data);
                stageData = JsonUtility.FromJson<StageData>(DataString);
                Debug.Log(DataString);
                //text.text = DataString;
            }

            //데이타 읽기 성공 
            //LoadingObj.SetActive(false);
        }
        else
        {
            //데이타 읽기 실패 
        }
    }

    #endregion


    private void Start()
    {
        isReadyToLogIn = 0;
        stageData = new StageData();
        StartCoroutine("LogIn");

    }
}


[Serializable]
public class stageInnerData
{
    public string stage;
    public int enable;
}



[Serializable]
public class StageData
{
    public stageInnerData[] StageInnerData;

}


