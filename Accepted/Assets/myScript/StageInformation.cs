using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformation : MonoBehaviour
{
    public GameObject StageText, PointText;
    public int TotalStage = 0, idx = 0;
    public GoogleManager GoogleManager;
    StageData stageData;

    private void Start()
    {
        int Pointidx = 0, realCount = 0;
        GameObject Google = GameObject.Find("GameManager");
        stageData = stageData = Google.GetComponent<GoogleManager>().stageData;
        GoogleManager = Google.GetComponent<GoogleManager>();

        //while(stageData.StageInnerData[idx] != null && stageData.StageInnerData[idx].enable == 1 && realCount < TotalStage)
        //{
        //    Pointidx += stageData.StageInnerData[idx].point;
        //    idx++; realCount++;
        //}

        for(int i = idx; i < TotalStage; i++)
        {
            if(stageData.StageInnerData[i] != null)
            {
                Pointidx += stageData.StageInnerData[idx].point;

                if (stageData.StageInnerData[i].enable == 1)
                {
                    realCount++;
                }
            }
        }

        if (realCount == 0) realCount = 1;
        if (realCount == TotalStage) realCount = TotalStage + 1;

        StageText.GetComponent<Text>().text = realCount - 1 + " / " + (TotalStage - idx);
        PointText.GetComponent<Text>().text = Pointidx + " / " + (TotalStage - idx) * 3;

        
    }
}
