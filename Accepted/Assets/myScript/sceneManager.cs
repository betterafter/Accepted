using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Text.RegularExpressions;

public class sceneManager : MonoBehaviour
{

    // map
    public GameObject[ , ] minimap = new GameObject[30, 20];
    public bool[,] isUsed = new bool[30, 20];
    int[,] Direction = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

    public string stageName;
    public int stageLevel;
    public int dir;
    static int RestartButtonClickCnt = 0;

    public bool isBack = false, isChanged = false;
    

    public int[,] objColor = new int[5, 3];

    public bool IsRestart;
    public bool IsUndo;
    //public bool IsLastClickedButton_Undo;
    public bool IsLastClickButton_Move;
    public Sprite VerticalBar, HorizontalBar, CrossBar;
    //public Stack<UndoItem> stack = new Stack<UndoItem>();

    private GameObject player, obj;
    private Vector3 playerpos, objpos;

    private static GameObject gameManager, soundManager;
    public Sprite upSprite, DownSprite, LeftSprite, RightSprite, StepSprite;

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
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                minimap[i, j] = null; isUsed[i, j] = false;
            }
        }

        IsUndo = false;
        colorSave();
    }

    void Awake()
    {

        Advertisement.Initialize("3382566", false);

        //재시작 버튼 관련. 처음엔 chapter 명이 나오는 텍스트가 나오고 재시작 버튼을 눌렀을 땐 이게 나오면 안되서 설정.
        IsRestart = false;

        //게임의 여러 기능 관리 
        if (gameManager == null) 
        { 
            gameManager = GameObject.Find("GameManager");
            DontDestroyOnLoad(gameManager);
            gameManager.GetComponent<AudioSource>().Play();
        
        }

        if (soundManager == null) { soundManager = GameObject.Find("SoundManager"); DontDestroyOnLoad(soundManager); }

       



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
        //IsLastClickedButton_Undo = true;
    }


    private void Update()
    {

        // 스마트폰 뒤로가기 버튼 클릭  
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                isBack = true;
                Scene scene = SceneManager.GetActiveScene();
                if(scene.buildIndex == 0)
                {
                    Application.Quit();
                }
                else if(scene.name == "MapCreater")
                {
                    SceneManager.LoadScene(0);
                }
                else if(scene.name != "game" && scene.buildIndex > 1)
                {
                    SceneManager.LoadScene(1);
                }
                else if(scene.buildIndex == 1)
                {
                    SceneManager.LoadScene(scene.buildIndex - 1);
                }
            }
        }

        if(IsUndo)
        {
            for (int i = 0; i <= 20; i++)
            {
                for (int j = 0; j <= 15; j++)
                {
                    minimap[i, j] = null;
                }
            }
        }





        BarDisconnection();
        BarConnection();
    }


    private void BarDisconnection()
    {
        for (int i = 0; i <= 20; i++)
        {
            for (int j = 0; j <= 15; j++)
            {
                if(minimap[i, j] != null && minimap[i, j].tag.Contains("bar"))
                {
                    minimap[i, j].GetComponent<CollisionManager>().isConnected = false;
                }
            }
        }
    }

    private void BarConnection()
    {
        // 연결 여부 일단 확인 
        for(int i = 0; i <= 20; i++)
        {
            for(int j = 0; j <= 15; j++)
            {
                if(minimap[i, j] != null && minimap[i, j].CompareTag("brick") && minimap[i, j].GetComponent<CollisionManager>().isStepOn)
                {
                    for(int yy = 0; yy <= 20; yy++)
                    {
                        for(int xx = 0; xx <= 15; xx++)
                        {
                            isUsed[yy, xx] = false;
                        }
                    }

                    Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>();
                    q.Enqueue(new KeyValuePair<int, int>(i, j)); isUsed[i, j] = true;

                    while (q.Count != 0)
                    {
                        int first = q.Peek().Key, second = q.Peek().Value; q.Dequeue();

                        for (int k = 0; k < 4; k++)
                        {
                            int x = second + Direction[k, 0], y = first + Direction[k, 1];

                            if (x >= 0 && x <= 15 && y >= 0 && y <= 20 && !isUsed[y, x] && minimap[y, x] != null)
                            {
                                GameObject o = minimap[first, second], oo = minimap[y, x];

                                if ((k == 0 || k == 1) && (o.CompareTag("brick") || o.CompareTag("hobar") || o.CompareTag("crossbar")))
                                {
                                    if (oo != null && (oo.CompareTag("hobar") || oo.CompareTag("crossbar")))
                                    {
                                        oo.GetComponent<CollisionManager>().isConnected = true;
                                        isUsed[y, x] = true; q.Enqueue(new KeyValuePair<int, int>(y, x));
                                    }
                                }
                                else if ((k == 2 || k == 3) && (o.CompareTag("brick") || o.CompareTag("verbar") || o.CompareTag("crossbar")))
                                {
                                    if (oo != null && (oo.CompareTag("verbar") || oo.CompareTag("crossbar")))
                                    {
                                        oo.GetComponent<CollisionManager>().isConnected = true;
                                        isUsed[y, x] = true; q.Enqueue(new KeyValuePair<int, int>(y, x));
                                    }
                                }
                            }
                        }
                    }


                }
            }
        }

        for (int i = 0; i <= 20; i++)
        {
            for (int j = 0; j <= 15; j++)
            {
                if (minimap[i, j] != null && minimap[i, j].tag.Contains("bar"))
                {
                    if(minimap[i, j].GetComponent<CollisionManager>().isConnected)
                    {
                        if(minimap[i, j].CompareTag("hobar"))
                        {
                            minimap[i, j].GetComponent<SpriteRenderer>().sprite = HorizontalBar;
                            minimap[i, j].transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else if (minimap[i, j].CompareTag("verbar"))
                        {
                            minimap[i, j].GetComponent<SpriteRenderer>().sprite = VerticalBar;
                            minimap[i, j].transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else if (minimap[i, j].CompareTag("crossbar"))
                        {
                            minimap[i, j].GetComponent<SpriteRenderer>().sprite = CrossBar;
                            minimap[i, j].transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        minimap[i, j].GetComponent<SpriteRenderer>().sprite = minimap[i, j].GetComponent<CollisionManager>().bar;
                        minimap[i, j].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }


}


