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

    GameObject Point1, Point2, Point3;
    GameObject End_RetryButton, End_ExitButton, End_NextSceneButton;

    private StageData EndStageData;
    private sceneManager s;
    private GoogleManager googleManager;


    public int ReadyToClear, last, lastscale, inObj;
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

    void isStepOn()
    {
        bool isReady = true;

        for (int i = 0; i < s.stepList.Count; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if (s.StepName[j] != null && s.StepName[j] != "" && s.stepList[i].CompareTag(s.StepName[j]))
                {
                    if (!s.stepList[i].GetComponent<StepCollision>().isStep)
                    {
                        Debug.Log(s.stepList[i].transform.position.x + "," + s.stepList[i].transform.position.y);
                        isReady = false; break;
                    }
                }
            }
            if (!isReady)
            {
                ReadyToClear = 0; break;
            }

            if (i == s.stepList.Count - 1)
            {
                ReadyToClear = 1;
            }
        }



        //for (int i = 0; i < s.stepList.Count; i++)
        //{
        //    bool isSwitch = false;

        //    if (!s.stepList[i].GetComponent<StepCollision>().isStep)
        //    {
        //        for(int j = 0; j < 10; j++)
        //        {
        //            if (s.isSwitch[j] != null && s.isSwitch[j] != "" && s.stepList[i].CompareTag(s.isSwitch[j]))
        //            {
        //                isSwitch = true; break;
        //            }
        //        }

        //        if (!isSwitch) break;
        //    }

        //    if (i == s.stepList.Count - 1)
        //    {
        //        currStepCnt = s.stepList.Count;
        //    }
        //}
    }


    private void Update()
    {

        isStepOn();

        ///////////lastCheck를 확인 하기 위한 설정///////////////
        //if (currStepCnt == stepCnt)
        //{
        //    ReadyToClear = 1;
        //}
        //else
        //{
        //    ReadyToClear = 0;
        //}

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
                for (int i = 0; i < 3; i++)
                {
                    if (!s.Point[i].GetComponent<SpriteRenderer>().enabled)
                    {
                        s.currPoint++;
                    }
                }

                GameObject.FindWithTag("Accepted").transform.GetChild(0).gameObject.SetActive(true);
                GameObject accepted = GameObject.Find("Accepted");

                Point1 = accepted.transform.Find("point1").gameObject;
                Point2 = accepted.transform.Find("point2").gameObject;
                Point3 = accepted.transform.Find("point3").gameObject;

                End_ExitButton = accepted.transform.GetChild(0).Find("stageExit").gameObject;
                End_RetryButton = accepted.transform.GetChild(0).Find("stageRetry").gameObject;
                End_NextSceneButton = accepted.transform.GetChild(0).Find("nextStage").gameObject;

                End_ExitButton.GetComponent<Button>()
                    .onClick.AddListener(delegate () { GameObject.FindWithTag("accepted").GetComponent<Stage>().QuitThisScene(); });

                End_RetryButton.GetComponent<Button>()
                    .onClick.AddListener(delegate () { GameObject.FindWithTag("accepted").GetComponent<Stage>().RestartThisScene(); });

                End_NextSceneButton.GetComponent<Button>()
                    .onClick.AddListener(delegate () { GameObject.FindWithTag("accepted").GetComponent<Stage>().NextScene(); });

                StartCoroutine("SetWeedColor");
                SceneChange();
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

                if(EndStageData.StageInnerData[idx].point <= s.currPoint)
                {
                    EndStageData.StageInnerData[idx].point = s.currPoint;
                }

                if(EndStageData.StageInnerData[idx + 1] != null)
                {
                    EndStageData.StageInnerData[idx + 1].enable = 1;
                }

                break;
            }

            idx++;
        }

        googleManager.SaveToCloud();
    }

    public void NextScene()
    {
        int idx = 0;
        while (idx < EndStageData.StageInnerData.Length)
        {
            if (s.stageName == EndStageData.StageInnerData[idx].stage)
            {
                break;
            }

            idx++;
        }

        if (EndStageData.StageInnerData[idx + 1] != null)
        {
            s.stageName = EndStageData.StageInnerData[idx + 1].stage;
            Debug.Log(s.stageName);
            SceneManager.LoadScene("game");
        }
        else
        {
            SceneManager.LoadScene("stageSelect");
        }
    }

    public void RestartThisScene()
    {
        SceneManager.LoadScene("game");
    }

    public void QuitThisScene()
    {
        //게임을 클리어하면 스테이지 선택창으로 넘어가게 됨.
        SceneManager.LoadScene("stageSelect");
    }

    IEnumerator weedColor(GameObject obj)
    {
        Color color = obj.GetComponent<Image>().color;
        while(color.a <= 1)
        {
            color.a += 0.1f;
            obj.GetComponent<Image>().color = color;
            yield return null;
        }
    }

    IEnumerator SetWeedColor()
    {
        if(s.currPoint == 1)
        {
            StartCoroutine(weedColor(Point1));
        }
        else if(s.currPoint == 2)
        {
            while (true)
            {
                StartCoroutine(weedColor(Point1));
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(weedColor(Point2));

                break;
            }
        }
        else if(s.currPoint == 3)
        {
            while (true)
            {
                StartCoroutine(weedColor(Point1));
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(weedColor(Point2));
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(weedColor(Point3));
                break;
            }
        }
    }
}
