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
    private GameObject gamebutton;

    static string LINEPARSE = @"\n";
    static string ATTRIBUTEPARSE = @",";

    private void Awake()
    {
        gamebutton = GameObject.Find("gamebutton");
    }


    private void Start()
    {
        stepCnt = 0;
        sceneManager s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        string mapName = "map/stage" + s.stageName;

        csvParser(mapName);


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

        else set();
        //find();
    }

    private void set()
    {
        //chapter 타이틀이 실행이 한번 되면 이제 다시 꺼지게 됨.
        GameObject.Find("chaptertext").SetActive(false);
        GameObject.Find("title").SetActive(false);

        //게임이 시작하면 UI 버튼 활성화
        gamebutton.transform.Find("retry").gameObject.SetActive(true);
        Button btn = GameObject.Find("retry").GetComponent<Button>();
        btn.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().RestartClick(); });

        gamebutton.transform.Find("undo").gameObject.SetActive(true);
        Button btn2 = GameObject.Find("undo").GetComponent<Button>();
        btn2.onClick.AddListener(delegate () { GameObject.Find("GameManager").GetComponent<sceneManager>().UndoClick(); });

        gamebutton.transform.Find("up").gameObject.SetActive(true);
        Button btn3 = GameObject.Find("up").GetComponent<Button>();
        btn3.onClick.AddListener(delegate () { GameObject.FindWithTag("player").GetComponent<PlayerController>().Move(btn3); });

        gamebutton.transform.Find("down").gameObject.SetActive(true);
        Button btn4 = GameObject.Find("down").GetComponent<Button>();
        btn4.onClick.AddListener(delegate () { GameObject.FindWithTag("player").GetComponent<PlayerController>().Move(btn4); });

        gamebutton.transform.Find("left").gameObject.SetActive(true);
        Button btn5 = GameObject.Find("left").GetComponent<Button>();
        btn5.onClick.AddListener(delegate () { GameObject.FindWithTag("player").GetComponent<PlayerController>().Move(btn5); });

        gamebutton.transform.Find("right").gameObject.SetActive(true);
        Button btn6 = GameObject.Find("right").GetComponent<Button>();
        btn6.onClick.AddListener(delegate () { GameObject.FindWithTag("player").GetComponent<PlayerController>().Move(btn6); });

        gamebutton.transform.Find("backStage").gameObject.SetActive(true);

        find();

        //발판 개수를 저장해줌. 게임 클리어 때 발판 개수랑 밟힌 개수랑 비교해서 클리어 여부를 판단. 
        Stage s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
        s.stepCnt = stepCnt;
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
        for(int i = 0; i < obj.Count - 1; i++)
        {
            int x = System.Convert.ToInt32(obj[i][1]);
            int y = System.Convert.ToInt32(obj[i][2]);
            string path = "Prefabs/obj/" + obj[i][0];

            //Debug.Log(obj[i][0]);

            if (obj[i][0].Contains("step"))
            {
                //Debug.Log("!!");
                GameObject o = Resources.Load(path) as GameObject;
                GameObject b = Instantiate(o, new Vector2(x, y), Quaternion.identity);
                b.tag = obj[i][0];

                stepCnt++;
            }

            else
            {
                GameObject o = Resources.Load(path) as GameObject;
                GameObject b = Instantiate(o, new Vector2(x, y), Quaternion.identity);
                b.tag = obj[i][0];
            }
        }
    }







}
