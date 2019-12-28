using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Map_Editor : MonoBehaviour
{
    List<List<string>> obj = new List<List<string>>();

    private Text chaptertxt, titletxt;
    private int stepCnt;
    private Stage stage;
    private bool Up, Down, Left, Right;

    public GameObject gamebutton, gamebutton2;
    public GameObject Wall;
    public Transform LeftBtntrans, RightBtntrans, UpBtntrans, DownBtntrans;
    public Button Leftbtn, Rightbtn, Upbtn, Downbtn;

    static string LINEPARSE = @"\n";
    static string ATTRIBUTEPARSE = @",";

    private void Awake()
    {
        gamebutton = GameObject.Find("gamebutton");
        gamebutton2 = GameObject.Find("gamebutton2");
    }


    private void Start()
    {
        stepCnt = 0;
        sceneManager s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        Wall = GameObject.Find("wall");
        string mapName = "map/stage" + s.stageName;

        csvParser(mapName);

        //시작할 때 방향키 발판 및 블럭이 존재하는지 체크하기 위한 초기화 코드 
        Up = false; Down = false; Left = false; Right = false;

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
        }

        //재시작을 누른거라면 : 그냥 오브젝트 재배치  
        else if (s.IsRestart == true) set();
    }

    // 인게임 내에서의 각종 UI 활성화 
    private void set()
    {
        //chapter 타이틀이 실행이 한번 되면 이제 다시 꺼지게 됨.
        GameObject.Find("chaptertext").SetActive(false);
        GameObject.Find("title").SetActive(false);
        Wall.transform.Find("wall").gameObject.SetActive(true);

        //게임이 시작하면 UI 버튼 활성화 //////////////////////////////////////////////////////////////////////////////////////////////
        gamebutton2.transform.Find("retry").gameObject.SetActive(true);
        Button btn = GameObject.Find("retry").GetComponent<Button>();
        btn.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().RestartClick(); });

        gamebutton2.transform.Find("undo").gameObject.SetActive(true);
        Button btn2 = GameObject.Find("undo").GetComponent<Button>();
        btn2.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().UndoClick(); });

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
    }


    private void Update()
    {
        //방향키 활성화 여부를 실시간으로 체크//////////////////////////////////////////////
        if (stage != null)
        {
            if (!Up) UpBtntrans.gameObject.SetActive(true);
            else if (Up)
            {
                if (stage.IsUpActive > 0) UpBtntrans.gameObject.SetActive(true);
                else UpBtntrans.gameObject.SetActive(false);
            }

            if (!Down) DownBtntrans.gameObject.SetActive(true);
            else if (Down)
            {
                if (stage.IsDownActive > 0) DownBtntrans.gameObject.SetActive(true);
                else DownBtntrans.gameObject.SetActive(false);
            }

            if (!Left) LeftBtntrans.gameObject.SetActive(true);
            else if (Left)
            {
                if (stage.IsLeftActive > 0) LeftBtntrans.gameObject.SetActive(true);
                else LeftBtntrans.gameObject.SetActive(false);
            }

            if (!Right) RightBtntrans.gameObject.SetActive(true);
            else if (Right)
            {
                if (stage.IsRightActive > 0) RightBtntrans.gameObject.SetActive(true);
                else RightBtntrans.gameObject.SetActive(false);
            }
        }
        /////////////////////////////////////////////////////////////////////////
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
            //data.Clear();
            string[] attr = Regex.Split(line[i], ATTRIBUTEPARSE);
            List<string> data = new List<string>();
            //Debug.Log(line[i]);
            for(int j = 0; j < attr.Length; j++)
            {
                data.Add(attr[j]);
            }
            
            obj.Add(data);
        }


        return obj;
    }





    public void find()
    {
        sceneManager scenemanager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        for(int i = 0; i < obj.Count - 1; i++)
        {
            int x = System.Convert.ToInt32(obj[i][1]);
            int y = System.Convert.ToInt32(obj[i][2]);

            int R = scenemanager.objColor[scenemanager.stageLevel, 0];
            int G = scenemanager.objColor[scenemanager.stageLevel, 1];
            int B = scenemanager.objColor[scenemanager.stageLevel, 2];

            string path = "Prefabs/obj/" + obj[i][0];

            if (obj[i][0].Contains("step"))
            {

                GameObject o = Resources.Load(path) as GameObject;
                GameObject b = Instantiate(o, new Vector3(x, y), Quaternion.identity);
                b.GetComponent<SpriteRenderer>().color = new Color(R / 255f, G / 255f, B / 255f);

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
                    b.GetComponent<SpriteRenderer>().color = new Color(R / 255f, G / 255f, B / 255f);
                }

                if (obj[i][0] == "robot")
                {
                    b.GetComponent<PlayerController>().enabled = false;
                    b.tag = obj[i][0];
                }
                else b.tag = obj[i][0];
            }
        }
    }







}
