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
    public bool[] DirectionKeyActivation = new bool[4];
    public bool[] isDirectionKey = new bool[4];
    public bool[] rotatebool = new bool[3];

    public string[] isSwitch = new string[3];
    public string[] StepName = new string[6];
    public Sprite[] StepSprite = new Sprite[10];

    public Sprite point1, point2, point3, point0;

    int[,] Direction = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 } };

    public string stageName;
    public int stageLevel;
    public int dir, currPoint;
    static int RestartButtonClickCnt = 0;

    public bool isChanged = false;


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
    //public Sprite upSprite, DownSprite, LeftSprite, RightSprite, StepSprite;

    public List<GameObject> stepList = new List<GameObject>();

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
        reload();
    }

    public void reload()
    {
        stepList = new List<GameObject>();
        rotatebool[0] = false; rotatebool[1] = false; rotatebool[2] = false;

        DirectionKeyActivation[0] = true; DirectionKeyActivation[1] = true;
        DirectionKeyActivation[2] = true; DirectionKeyActivation[3] = true;
        isDirectionKey[0] = false; isDirectionKey[1] = false;
        isDirectionKey[2] = false; isDirectionKey[3] = false;

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                minimap[i, j] = null; isUsed[i, j] = false;
            }
        }

        IsUndo = false;
        colorSave();
    }


    void Awake()
    {
        int w = Screen.width, h = Screen.height;

        rect = new Rect(0, 0, w, h * 4 / 100);

        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 4 / 400;
        style.normal.textColor = Color.cyan;

        StartCoroutine("worstReset");


        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        StepName[0] = "brickstep"; StepName[1] = "upstep"; StepName[2] = "downstep";
        StepName[3] = "leftstep"; StepName[4] = "rightstep"; StepName[5] = "rotatestep";

        isSwitch[0] = "rotatestep"; isSwitch[1] = "accepted"; isSwitch[2] = "clonespawner";
        //stepList.Clear();
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

        stepList = new List<GameObject>();
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
    }



    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        // 스마트폰 뒤로가기 버튼 클릭  
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    if (SceneManager.GetActiveScene().name != "game")
        //    {
        //        if (Input.GetKey(KeyCode.Escape))
        //        {
        //            QuitUI.SetActive(true);
        //        }
        //    }
        //    else if (SceneManager.GetActiveScene().name == "game")
        //    {
        //        if (Input.GetKey(KeyCode.Escape))
        //        {
        //            StageQuitUI.SetActive(true);
        //        }
        //    }
        //}
         
        if (SceneManager.GetActiveScene().name == "game")
        {
            BarDisconnection();
            BarConnection();
            DirectionButtonActivation();
            RotateStepActivation();


        }
    }

    void RotateStepActivation()
    {
        int stepCount = 0;

        if(stepList.Count > 0)
        {
            for(int i = 0; i < stepList.Count; i++)
            {
                if (stepList[i].CompareTag("rotatestep") && stepList[i].GetComponent<StepCollision>().isStep)
                {
                    stepCount++;
                }
            }

            if (stepCount >= 3) { rotatebool[2] = true; rotatebool[1] = false; rotatebool[0] = false; }
            else if (stepCount == 2) { rotatebool[2] = false; rotatebool[1] = true; rotatebool[0] = false; }
            else if (stepCount == 1) { rotatebool[2] = false; rotatebool[1] = false; rotatebool[0] = true; }
            else if (stepCount == 0) { rotatebool[2] = false; rotatebool[1] = false; rotatebool[0] = false; }
        }
    }

    void DirectionButtonActivation()
    {
        if (stepList.Count > 0)
        {
            for (int i = 0; i < stepList.Count; i++)
            {
                if (stepList[i].CompareTag("upstep"))
                {
                    isDirectionKey[0] = true;
                    if (stepList[i].GetComponent<StepCollision>().isStep)
                    {
                        DirectionKeyActivation[0] = true;
                        break;
                    }
                }
                if (i == stepList.Count - 1) DirectionKeyActivation[0] = false;
            }

            for (int i = 0; i < stepList.Count; i++)
            {
                if (stepList[i].CompareTag("downstep"))
                {
                    isDirectionKey[1] = true;
                    if (stepList[i].GetComponent<StepCollision>().isStep)
                    {
                        DirectionKeyActivation[1] = true;
                        break;
                    }
                }
                if (i == stepList.Count - 1) DirectionKeyActivation[1] = false;
            }

            for (int i = 0; i < stepList.Count; i++)
            {
                if (stepList[i].CompareTag("leftstep"))
                {
                    isDirectionKey[2] = true;
                    if (stepList[i].GetComponent<StepCollision>().isStep)
                    {
                        DirectionKeyActivation[2] = true;
                        break;
                    }
                }
                if (i == stepList.Count - 1) DirectionKeyActivation[2] = false; 
            }

            for (int i = 0; i < stepList.Count; i++)
            {
                if (stepList[i].CompareTag("rightstep"))
                {
                    isDirectionKey[3] = true;
                    if (stepList[i].GetComponent<StepCollision>().isStep)
                    {
                        DirectionKeyActivation[3] = true;
                        break;
                    }
                }
                if (i == stepList.Count - 1) DirectionKeyActivation[3] = false;
            }
        }
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

                                if((k == 0 || k == 1) && o.CompareTag("brick"))
                                {
                                    if (oo != null && oo.CompareTag("hobar"))
                                    {
                                        oo.GetComponent<CollisionManager>().isConnected = true;
                                        isUsed[y, x] = true; q.Enqueue(new KeyValuePair<int, int>(y, x));
                                    }
                                }

                                if ((k == 2 || k == 3) && o.CompareTag("brick"))
                                {
                                    if (oo != null && oo.CompareTag("verbar"))
                                    {
                                        oo.GetComponent<CollisionManager>().isConnected = true;
                                        isUsed[y, x] = true; q.Enqueue(new KeyValuePair<int, int>(y, x));
                                    }
                                }

                                if ((k == 0 || k == 1) && (o.CompareTag("hobar")))
                                {
                                    if (oo != null && (oo.CompareTag("hobar") || oo.CompareTag("brick")))
                                    {
                                        oo.GetComponent<CollisionManager>().isConnected = true;
                                        isUsed[y, x] = true; q.Enqueue(new KeyValuePair<int, int>(y, x));
                                    }
                                }
                                else if ((k == 2 || k == 3) && (o.CompareTag("verbar")))
                                {
                                    if (oo != null && (oo.CompareTag("verbar") || oo.CompareTag("brick")))
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
                        if(minimap[i, j].GetComponent<ObjectStatus>().Currtag == "hobar")
                        {
                            minimap[i, j].GetComponent<SpriteRenderer>().sprite = HorizontalBar;
                            minimap[i, j].transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else if (minimap[i, j].GetComponent<ObjectStatus>().Currtag == "verbar")
                        {
                            minimap[i, j].GetComponent<SpriteRenderer>().sprite = VerticalBar;
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



    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    string text;



    IEnumerator worstReset() //코루틴으로 15초 간격으로 최저 프레임 리셋해줌.
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            worstFps = 100f;
        }
    }


    //void Update()
    //{
    //    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    //}

    void OnGUI()//소스로 GUI 표시.
    {

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  //초당 프레임 - 1초에

        if (fps < worstFps)  //새로운 최저 fps가 나왔다면 worstFps 바꿔줌.
            worstFps = fps;
        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
        GUI.Label(rect, text, style);
    }
}


