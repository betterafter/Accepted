using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Text.RegularExpressions;

public class sceneManager : MonoBehaviour
{

    public string stageName;
    public int stageLevel;
    public int dir;
    static int RestartButtonClickCnt = 0;

    public int[,] objColor = new int[5, 3];

    public bool IsRestart;
    public bool IsUndo;
    public bool IsLastClickedButton_Undo;
    public bool IsLastClickButton_Move;
    //public Stack<UndoItem> stack = new Stack<UndoItem>();

    private GameObject player, obj;
    private Vector3 playerpos, objpos;


    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }
    }

    private void colorSave()
    {

        string ColorPath = "map/mapColor";
        TextAsset mapColor = Resources.Load(ColorPath) as TextAsset;

        string[] txt = Regex.Split(mapColor.text, @"\n");
        if (txt.Length < 1) return;
        for(int i = 0; i < txt.Length; i++)
        {
            string[] lineTxt = Regex.Split(txt[i], @",");
            for(int j = 0; j < lineTxt.Length; j++)
            {
                objColor[i + 1, j] = System.Convert.ToInt32(lineTxt[j]);
            }
        }
    }


    private void Start()
    {
        IsUndo = false;
        colorSave();
    }

    void Awake()
    {

        Advertisement.Initialize("3382566", false);

        //재시작 버튼 관련. 처음엔 chapter 명이 나오는 텍스트가 나오고 재시작 버튼을 눌렀을 땐 이게 나오면 안되서 설정.
        IsRestart = false;

        //게임의 여러 기능 관리 
        GameObject gamemanger = GameObject.Find("GameManager");
        DontDestroyOnLoad(gamemanger);

        //멀티터치가 발생하면 벽돌을 뚫고 가는 현상이 있음 
        Input.multiTouchEnabled = false;
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("game");
        if (RestartButtonClickCnt < 4) RestartButtonClickCnt++;
        else if(RestartButtonClickCnt == 4)
        {
            ShowAd();
            RestartButtonClickCnt = 0;
        }
        IsRestart = true;
    }

    public void UndoClick()
    {
        IsUndo = true;
        IsLastClickedButton_Undo = true;
    }

    private void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Scene scene = SceneManager.GetActiveScene();
                if(scene.buildIndex == 0)
                {
                    Application.Quit();
                }
                else if(scene.name == "MapCreater")
                {
                    SceneManager.LoadScene(0);
                }
                else if(scene.name == "game")
                {
                    SceneManager.LoadScene(stageLevel.ToString());
                }
                else if(scene.buildIndex > 1)
                {
                    SceneManager.LoadScene(1);
                }
                else if(scene.buildIndex == 1)
                {
                    SceneManager.LoadScene(scene.buildIndex - 1);
                }
            }

        }
    }

}


