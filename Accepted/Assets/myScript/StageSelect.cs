using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class StageSelect : MonoBehaviour
{

    public StageData stageDataToSelect;
    public sceneManager s;

    private void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { this.GetComponent<StageSelect>().StageClick(); });

        stageDataToSelect = GameObject.Find("GameManager").GetComponent<GoogleManager>().stageData;
        s = GameObject.Find("GameManager").GetComponent<sceneManager>();

        int idx = (s.stageLevel - 1) * 17, EndIdx = idx + 16;
        while (idx <= EndIdx)
        {
            if (this.gameObject.name == stageDataToSelect.StageInnerData[idx].stage)
            {
                GameObject Lockobj = GameObject.Find(this.gameObject.name + "Lock");

                if (stageDataToSelect.StageInnerData[idx].enable == 0)
                {
                    Lockobj.SetActive(true);
                }
                else
                {
                    Lockobj.SetActive(false);
                }

                break;
            }
            idx++;
        }
    }


    public void StageClick()
    {
        s.stageName = this.gameObject.name;
        s.IsRestart = false;

        SceneManager.LoadScene("game");
        
    }
}
