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
    sceneManager SceneManager;

    void Start()
    {
        Text text = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = gameObject.name;

        GameManager = GameObject.Find("GameManager");
        stageData = GameManager.GetComponent<GoogleManager>().stageData;
        StageSelect = Camera.main.GetComponent<StageSelect>();
        SceneManager = GameManager.GetComponent<sceneManager>();

        //GameManager.GetComponent<GoogleManager>().LoadFromCloud();

        int idx = 0;
        while(idx < stageData.StageInnerData.Length)
        {
            if (stageData.StageInnerData[idx] != null)
            {
                if(stageData.StageInnerData[idx].stage == gameObject.name)
                {
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
                    if(stageData.StageInnerData[idx].enable == 1 && stageData.StageInnerData[idx + 1] != null
                         && stageData.StageInnerData[idx + 1].enable == 0)
                    {
                        gameObject.GetComponent<Image>().sprite = StageSelect.EnableSprite;
                        gameObject.GetComponent<Image>().color = new Color(119 / 255f, 255 / 255f, 165 / 255f);
                        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;

                    }

                    break;
                }
            }
            idx++;
        }

        if (gameObject.transform.GetChild(1) != null)
        {
            gameObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 43);
            gameObject.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(30, 30);

            if (stageData.StageInnerData[idx].point == 1)
            {
                gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = SceneManager.point1;
            }
            else if (stageData.StageInnerData[idx].point == 2)
            {
                gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = SceneManager.point2;
            }
            else if (stageData.StageInnerData[idx].point == 3)
            {
                gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = SceneManager.point3;
            }
            else if (stageData.StageInnerData[idx].point == 0)
            {
                gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = SceneManager.point0;
            }
        }
    }


}
