using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class stageLevelSelect : MonoBehaviour
{
    public sceneManager s;

    public void StageLevelClick()
    {
        s = GameObject.Find("GameManager").GetComponent<sceneManager>();

        s.stageLevel = Convert.ToInt32(this.gameObject.name);
        SceneManager.LoadScene(this.gameObject.name);
    }

    public void BackToStageSelect()
    {
        SceneManager.LoadScene("stageSelect");
    }

    public void BackToStageLevel()
    {
        s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        SceneManager.LoadScene(s.stageLevel.ToString());
    }
}
