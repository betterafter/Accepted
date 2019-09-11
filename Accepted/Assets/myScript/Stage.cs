using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public int stepCnt, currStepCnt;
    public int IsLeftActive, IsRightActive, IsUpActive, IsDownActive;
    public int CurrentRotation;
    public GameObject manager;

    private StageData EndStageData;
    private sceneManager s;
    private GoogleManager googleManager;


    private int ReadyToClear, last, lastscale, inObj;
    private Vector3 scale;
    private GameObject finalobj;
    Animator anime;


    void Start()
    {
        manager = GameObject.Find("GameManager");
        googleManager = manager.GetComponent<GoogleManager>();
        s = manager.GetComponent<sceneManager>();
        EndStageData = googleManager.stageData;

        last = 0; lastscale = 0; inObj = 0;
        ReadyToClear = 0;

        StartCoroutine("LastCheck");
    }


    private void Update()
    {
     
        ///////////lastCheck를 확인 하기 위한 설정///////////////
        if (currStepCnt == stepCnt)
        {
            ReadyToClear = 1;
        }
        else
        {
            ReadyToClear = 0;
        }

        if(finalobj != null)
        {
            anime = finalobj.GetComponent<Animator>();
            scale = finalobj.transform.localScale;

            if (anime.GetBool("IsIdle") == true && finalobj.GetComponent<PlayerController>().IsClone == 0) last = 1;
            else last = 0;

            if ((int)scale.x == 1) lastscale = 1;
            else lastscale = 0;
            //Debug.Log(last + "," + lastscale);
        }
        ////////////////////////////////////////////////////
    }

    //스테이지 클리어인지 여부를 코루틴으로 계속 확인
    IEnumerator LastCheck()
    {
        while (true)
        {
            if (inObj == 1 && ReadyToClear == 1 && lastscale == 1 && last == 1)
            {
                StartCoroutine("Clear");
                Invoke("SceneChange", 4f);
                break;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    ////player가 accepted를 밟았는지 아닌지 체크///
    private void OnTriggerExit2D(Collider2D collision)
    {
        finalobj = null;
        inObj = 0;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            finalobj = collision.gameObject;
            inObj = 1;
        }
    }

    //////////////////////////////////

  

    public void SceneChange()
    {
        int idx = 0;
        while (idx < EndStageData.StageInnerData.Length)
        {
            if (s.stageName == EndStageData.StageInnerData[idx].stage)
            {
                if(EndStageData.StageInnerData[idx + 1] != null)
                {
                    EndStageData.StageInnerData[idx + 1].enable = 1;
                }

                break;
            }

            idx++;
        }
        googleManager.SaveToCloud();

        //게임을 클리어하면 스테이지 선택창으로 넘어가게 됨.
        SceneManager.LoadScene(s.stageLevel.ToString());
    }


    IEnumerator Clear()
    {
        //게임을 클리어하면 클리어했다는 멘트가 나옴.
        GameObject.FindWithTag("Accepted").transform.Find("Panel").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        GameObject.FindWithTag("Accepted").transform.Find("accepted!").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        GameObject.FindWithTag("Accepted").transform.Find("stageclear").gameObject.SetActive(true);
    }


    //float deltaTime = 0.0f;

    //GUIStyle style;
    //Rect rect;
    //float msec;
    //float fps;
    //float worstFps = 100f;
    //string text;

    //void Awake()
    //{
    //    int w = Screen.width, h = Screen.height;

    //    rect = new Rect(0, 0, w, h * 4 / 100);

    //    style = new GUIStyle();
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 4 / 100;
    //    style.normal.textColor = Color.cyan;

    //    StartCoroutine("worstReset");
    //}


    //IEnumerator worstReset() //코루틴으로 15초 간격으로 최저 프레임 리셋해줌.
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(15f);
    //        worstFps = 100f;
    //    }
    //}


    //void Update()
    //{
    //    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    //}

    //void OnGUI()//소스로 GUI 표시.
    //{

    //    msec = deltaTime * 1000.0f;
    //    fps = 1.0f / deltaTime;  //초당 프레임 - 1초에

    //    if (fps < worstFps)  //새로운 최저 fps가 나왔다면 worstFps 바꿔줌.
    //        worstFps = fps;
    //    text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
    //    GUI.Label(rect, text, style);
    //}

}
