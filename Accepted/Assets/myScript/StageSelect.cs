using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelect : MonoBehaviour
{

    public StageData stageDataToSelect;

    private void Start()
    {
        stageDataToSelect = GameObject.Find("GameManager").GetComponent<GoogleManager>().stageData;
        Debug.Log(stageDataToSelect.StageInnerData[0].stage);

        int idx = 0;
        while(idx < stageDataToSelect.StageInnerData.Length)
        {
            if(this.gameObject.name == stageDataToSelect.StageInnerData[idx].stage)
            {
                if(stageDataToSelect.StageInnerData[idx].enable == 0)
                {
                    this.gameObject.SetActive(false);
                }

                break;
            }

            idx++;
        }

        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { this.GetComponent<StageSelect>().StageClick(); });
    }


    public void StageClick()
    {
        sceneManager s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        s.stageName = this.gameObject.name;
        s.IsRestart = false;

        SceneManager.LoadScene("game");
    }
}
