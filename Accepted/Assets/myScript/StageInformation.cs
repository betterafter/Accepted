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
        int Pointidx = 0;
        GameObject Google = GameObject.Find("GameManager");
        stageData = stageData = Google.GetComponent<GoogleManager>().stageData;
        GoogleManager = Google.GetComponent<GoogleManager>();

        while(stageData.StageInnerData[idx] != null && stageData.StageInnerData[idx].enable == 1)
        {
            Pointidx += stageData.StageInnerData[idx].point;
            idx++;
        }

        StageText.GetComponent<Text>().text = idx + " / " + TotalStage;
        PointText.GetComponent<Text>().text = Pointidx + " / " + TotalStage * 3;
    }
}
