using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Map_Editor : MonoBehaviour
{
    List<List<string>> obj = new List<List<string>>();
    sceneManager s;

    private Text chaptertxt, titletxt;
    private int stepCnt;
    private Stage stage;
    private bool Up, Down, Left, Right;

    public GameObject gamebutton, gamebutton2, ChapterIntroBackground, UIimage, uptitle;
    public GameObject Wall, player;
    public Transform LeftBtntrans, RightBtntrans, UpBtntrans, DownBtntrans;
    public Button Leftbtn, Rightbtn, Upbtn, Downbtn;

    string MapName;
    public GameObject Panel, Image, Button, InformationCanvas, subInformation;

    static string LINEPARSE = @"\n";
    static string ATTRIBUTEPARSE = @",";

    private void Awake()
    {
        gamebutton = GameObject.Find("gamebutton");
        gamebutton2 = GameObject.Find("gamebutton2");
    }


    private void Start()
    {
        MakeTile(); 
        UIimage.SetActive(true); 
        stepCnt = 0;
        s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        string mapName = "map/stage" + s.stageName;
        MapName = s.stageName;
        uptitle.GetComponent<Text>().text = s.stageName;
        uptitle.SetActive(true);

        csvParser(mapName);
        s.reload(); s.currPoint = 0;

        //시작할 때 방향키 발판 및 블럭이 존재하는지 체크하기 위한 초기화 코드 
        Up = false; Down = false; Left = false; Right = false;
        //if (!s.IsRestart) Invoke("OpenInformation", 4.5f);

        //재시작 버튼을 눌렀는지 여부 확인
        //재시작을 누른게 아닌 처음 시작이라면 : 인트로 화면 시작 후 오브젝트 배치 
        if (s.IsRestart == false)
        {
            chaptertxt = GameObject.Find("chaptertext").GetComponent<Text>();
            titletxt = GameObject.Find("title").GetComponent<Text>();

            chaptertxt.text = obj[obj.Count - 1][0];
            titletxt.text = obj[obj.Count - 1][1];

            StartCoroutine("Fadechapter");
            StartCoroutine("Fadetitle");

            Invoke("set", 4f);
            //Invoke("Test", 5f);
        }

        //재시작을 누른거라면 : 그냥 오브젝트 재배치  
        else if (s.IsRestart == true) { set();  }

    }

    void Test()
    {

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = true;

        Upbtn.onClick.AddListener(delegate () { playerController.Move(Upbtn); });
        Downbtn.onClick.AddListener(delegate () { playerController.Move(Downbtn); });
        Leftbtn.onClick.AddListener(delegate () { playerController.Move(Leftbtn); });
        Rightbtn.onClick.AddListener(delegate () { playerController.Move(Rightbtn); });
    }

    // 인게임 내에서의 각종 UI 활성화 
    private void set()
    {
        //chapter 타이틀이 실행이 한번 되면 이제 다시 꺼지게 됨.
        GameObject.Find("chaptertext").SetActive(false);
        GameObject.Find("title").SetActive(false);
        ChapterIntroBackground.SetActive(false);
        //Wall.transform.Find("wall").gameObject.SetActive(true);


        //게임이 시작하면 UI 버튼 활성화 //////////////////////////////////////////////////////////////////////////////////////////////
        gamebutton2.transform.Find("retry").gameObject.SetActive(true);
        Button btn = GameObject.Find("retry").GetComponent<Button>();
        btn.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().RestartClick(); });

        gamebutton2.transform.Find("undo").gameObject.SetActive(true);
        Button btn2 = GameObject.Find("undo").GetComponent<Button>();
        btn2.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().UndoClick(); });

        gamebutton2.transform.Find("setting").gameObject.SetActive(true);
        Button btn2_1 = GameObject.Find("setting").GetComponent<Button>();
        btn2_1.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<GameUIManager>().Setting(); });

        UpBtntrans = gamebutton.transform.Find("up");
        UpBtntrans.gameObject.SetActive(true);
        Upbtn = GameObject.Find("up").GetComponent<Button>();

        DownBtntrans = gamebutton.transform.Find("down");
        DownBtntrans.gameObject.SetActive(true);
        Downbtn = GameObject.Find("down").GetComponent<Button>();

        LeftBtntrans = gamebutton.transform.Find("left");
        LeftBtntrans.gameObject.SetActive(true);
        Leftbtn = GameObject.Find("left").GetComponent<Button>();

        RightBtntrans = gamebutton.transform.Find("right");
        RightBtntrans.gameObject.SetActive(true);
        Rightbtn = GameObject.Find("right").GetComponent<Button>();



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //오브젝트 배치 함수 실행
        find();

        //발판 개수를 저장해줌. 게임 클리어 때 발판 개수랑 밟힌 개수랑 비교해서 클리어 여부를 판단. 
        stage = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        //방향키 활성화 여부 초기화 
        stage.IsLeftActive = 0; stage.IsRightActive = 0; stage.IsUpActive = 0; stage.IsDownActive = 0;

        //활성화된 발판 개수 초기화  
        stage.stepCnt = stepCnt;

        if(!s.IsRestart) OpenInformation();
        s.IsRestart = false;
    }

    private void MakeWall()
    {
        GameObject wall = Resources.Load("Prefabs/obj/wall") as GameObject;

        for (int i = -9; i <= 9; i++)
        {
            Instantiate(wall, new Vector3(i, 15), Quaternion.identity);
            Instantiate(wall, new Vector3(i, 16), Quaternion.identity);
            Instantiate(wall, new Vector3(i, -6), Quaternion.identity);
            Instantiate(wall, new Vector3(i, -7), Quaternion.identity);
        }

        for (int i = -5; i <= 14; i++)
        {
            Instantiate(wall, new Vector3(-9, i), Quaternion.identity);
            Instantiate(wall, new Vector3(-8, i), Quaternion.identity);
            Instantiate(wall, new Vector3(8, i), Quaternion.identity);
            Instantiate(wall, new Vector3(9, i), Quaternion.identity);
        }
    }

    private void MakeTile()
    {
        GameObject tile = Resources.Load("Prefabs/obj/tile") as GameObject;

        for(int i = -9; i <= 9; i++)
        {
            for(int j = -7; j <= 16; j++)
            {
                Instantiate(tile, new Vector3(i, j), Quaternion.identity);
            }
        }
    }

    public void CloseInformation()
    {
        Panel.SetActive(false); Image.SetActive(false); Button.SetActive(false);
        subInformation.SetActive(false);
    }

    void OpenInformation()
    {
        for (int idx = 0; idx < 100; idx++)
        {
            if (InformationCanvas.transform.GetChild(idx + 3).gameObject.name == "stage" + MapName)
            {
                Panel.SetActive(true); Image.SetActive(true); Button.SetActive(true);
                InformationCanvas.transform.GetChild(idx + 3).gameObject.SetActive(true);
                subInformation = InformationCanvas.transform.GetChild(idx + 3).gameObject;

                break;
            }
        }
    }


    private void Update()
    {
        if (UpBtntrans != null && DownBtntrans != null && LeftBtntrans != null && RightBtntrans != null)
        {
            if (s.DirectionKeyActivation[0] || !s.isDirectionKey[0]) UpBtntrans.gameObject.SetActive(true);
            else UpBtntrans.gameObject.SetActive(false);

            if (s.DirectionKeyActivation[1] || !s.isDirectionKey[1]) DownBtntrans.gameObject.SetActive(true);
            else DownBtntrans.gameObject.SetActive(false);

            if (s.DirectionKeyActivation[2] || !s.isDirectionKey[2]) LeftBtntrans.gameObject.SetActive(true);
            else LeftBtntrans.gameObject.SetActive(false);

            if (s.DirectionKeyActivation[3] || !s.isDirectionKey[3]) RightBtntrans.gameObject.SetActive(true);
            else RightBtntrans.gameObject.SetActive(false);
        }
    }


    IEnumerator Fadechapter()
    {
        Color cc = chaptertxt.color;

        while (true)
        {
            if (cc.a <= 1.0f)
            {
                cc.a += 0.1f;

                chaptertxt.color = cc;
            }
            yield return new WaitForSeconds(0.1f);
            //Debug.Log(cc.a);
        }
    }

    IEnumerator Fadetitle()
    {
        yield return new WaitForSeconds(1.5f);

        Color tc = titletxt.color;

        while (true)
        {
            if (tc.a <= 1.0f)
            {
                tc.a += 0.1f;

                titletxt.color = tc;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }






    private List<List<string>> csvParser(string mapName)
    {

        obj.Clear();
 
        TextAsset map = Resources.Load(mapName) as TextAsset;
        //Debug.Log(mapName);

        string[] line = Regex.Split(map.text, LINEPARSE);

        if (line.Length < 1) return obj;
        for(int i = 0; i < line.Length; i++)
        {
            string[] attr = Regex.Split(line[i], ATTRIBUTEPARSE);
            List<string> data = new List<string>();
            for(int j = 0; j < attr.Length; j++)
            {
                data.Add(attr[j]);
            }
            
            obj.Add(data);
        }


        return obj;
    }





    private void find()
    {
        sceneManager scenemanager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        for(int i = 0; i < obj.Count - 1; i++)
        {
            int x = System.Convert.ToInt32(obj[i][1]);
            int y = System.Convert.ToInt32(obj[i][2]);

            string path = "Prefabs/obj/" + obj[i][0];

            if (obj[i][0].Contains("step"))
            {

                GameObject o = Resources.Load(path) as GameObject;
                GameObject b = Instantiate(o, new Vector3(x, y), Quaternion.identity);
                //b.GetComponent<SpriteRenderer>().color = new Color(R / 255f, G / 255f, B / 255f);

                b.tag = obj[i][0];

                if (obj[i][0].Contains("up")) Up = true;
                else if (obj[i][0].Contains("down")) Down = true;
                else if (obj[i][0].Contains("left")) Left = true;
                else if (obj[i][0].Contains("right")) Right = true;

                stepCnt++;
            }

            else
            {
                GameObject o = Resources.Load(path) as GameObject;
                GameObject b = Instantiate(o, new Vector3(x, y), Quaternion.identity);

                if(obj[i][0] != "robot" && obj[i][0] != "player" && obj[i][0] != "researcher" && obj[i][0] != "CultureTube")
                {
                    //b.GetComponent<SpriteRenderer>().color = new Color(R / 255f, G / 255f, B / 255f);
                }

                if(obj[i][0] == "point")
                {
                    if(s.Point[0] == null)
                    {
                        s.Point[0] = b;
                    }
                    else if(s.Point[1] == null)
                    {
                        s.Point[1] = b;
                    }
                    else if(s.Point[2] == null)
                    {
                        s.Point[2] = b;
                    }
                }

                if (obj[i][0] == "robotplayer")
                {
                    PlayerController playerController = b.GetComponent<PlayerController>();

                    Upbtn.onClick.AddListener(delegate () { playerController.Move(Upbtn); });
                    Downbtn.onClick.AddListener(delegate () { playerController.Move(Downbtn); });
                    Leftbtn.onClick.AddListener(delegate () { playerController.Move(Leftbtn); });
                    Rightbtn.onClick.AddListener(delegate () { playerController.Move(Rightbtn); });

                    playerController.enabled = false;
                }

                else if(obj[i][0] == "player")
                {
                    player = b;
                }

                b.tag = obj[i][0];

                
            }
        }

        Invoke("Test", 0.5f);
    }








}
