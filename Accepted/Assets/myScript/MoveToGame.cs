using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MoveToGame : MonoBehaviour
{

    StageData stageData;
    StageSelect StageSelect;
    GameObject GameManager;

    void Start()
    {
        Text text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = gameObject.name;

        GameManager = GameObject.Find("GameManager");
        stageData = GameManager.GetComponent<GoogleManager>().stageData;
        StageSelect = Camera.main.GetComponent<StageSelect>();

        //GameManager.GetComponent<GoogleManager>().LoadFromCloud();

        int idx = 0;
        while(idx < stageData.StageInnerData.Length)
        {
            if (stageData.StageInnerData[idx] != null)
            {
                if(stageData.StageInnerData[idx].stage == gameObject.name)
                {
                    Debug.Log(stageData.StageInnerData[idx].stage);
                    Debug.Log(stageData.StageInnerData[idx].enable);

                    if (stageData.StageInnerData[idx].enable == 1)
                    {
                        gameObject.GetComponent<Image>().sprite = StageSelect.EnableSprite;
                        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
                    }
                    else if(stageData.StageInnerData[idx].enable == 0)
                    {
                        gameObject.GetComponent<Image>().sprite = StageSelect.DisableSprite;
                        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
                    }
                    break;
                }
            }
            idx++;
        }
    }
}
