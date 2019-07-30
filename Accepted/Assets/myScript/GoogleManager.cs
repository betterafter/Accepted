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
    //public Text username, STATUS;

    private bool isSaving;
    public bool saving, loading;



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

    public IEnumerator LogIn()
    {
        while (!Social.localUser.authenticated)
        {
            //Debug용
            //STATUS.text = "LOGIN";
            Social.localUser.Authenticate(SignInCallBack);

            yield return new WaitForSeconds(1f);
        }
    }

    //로그아웃기능은 사실상 필요없는 기능 
    //public void LogOut()
    //{
    //    if (!Social.localUser.authenticated)
    //    {
    //        PlayGamesPlatform.Instance.SignOut();

    //        //Debug용
    //        //STATUS.text = "logout";
    //    }
    //}

    public void Open_SavedData(string file, bool Save) 
    {
        //STATUS.text = "DEBUG TEST ON OPEN_SAVEDDATA";
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

        Open_SavedData(FILE_NAME, true);
    }

    private void OpenToSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            //이곳에 맵 클리어 여부 데이터를 바이트로 변환한 것을 넣으면 됨 

            string DataString = JsonUtility.ToJson(stageData);
            byte[] DataByte = Encoding.UTF8.GetBytes(DataString);

            SaveGame(game, DataByte, DateTime.Now.TimeOfDay);
        }
        else
        {
            //파일열기 실패
           // STATUS.text = "파일열기 실패1";
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

        }
        else
        {
            //데이터 저장 실패 
           // STATUS.text = "데이터 저장 실패";
        }
    }

    #endregion




    #region Load

    public void LoadFromCloud()
    {
        if (!Social.localUser.authenticated)
        {
            //STATUS.text = "LoadFromCloud fail";
            LogIn();
            return;
        }
        Open_SavedData("saveFile", false);
    }

    private void OpenToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
           // STATUS.text = "파일열기 성공2";
            LoadGame(game);
        }
        else
        {
            //파일 열기 실패 
           // STATUS.text = "파일열기 실패2";
        }
    }

    private void LoadGame(ISavedGameMetadata game)
    {
       // STATUS.text = "LoadGame Phase";
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, DataRead);
    }

    private void DataRead(SavedGameRequestStatus status, byte[] data)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            if(data.Length == 0)
            {

                TextAsset jsonString = Resources.Load("saveFile") as TextAsset;
                string LocalDataString = jsonString.text;

                //STATUS.text = LocalDataString;

                stageData = JsonUtility.FromJson<StageData>(LocalDataString);
                //JsonUtility.FromJsonOverwrite(LocalDataString, stageData);
            }
            else
            {
                string DataString = Encoding.UTF8.GetString(data);
                stageData = JsonUtility.FromJson<StageData>(DataString);
            }

            //데이타 읽기 성공 
          //  STATUS.text = "데이터 읽기 성공";
        }
        else
        {
            //데이타 읽기 실패 
          //  STATUS.text = "데이터 읽기 실패";
        }
    }

    #endregion


    private void Start()
    {

        stageData = new StageData();

        StartCoroutine("LogIn");
        Invoke("TEST2", 5f);


    }


    public void TEST2()
    {
        //SaveToCloud();
        LoadFromCloud();
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


