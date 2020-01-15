using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    StageSelect stageSelect;
    sceneManager sceneManager;

    AudioSource audioSource;


    private void Start()
    {
        stageSelect = Camera.main.GetComponent<StageSelect>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();

        if(gameObject.CompareTag("stage"))
            gameObject.GetComponent<Button>().onClick.AddListener(delegate () { gameObject.GetComponent<StageSelectButton>().StageClick(); });
    }


    public void FloorClick()
    {
        stageSelect.targetY = this.gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition.y;

        stageSelect.targetUI = Convert.ToInt32(this.gameObject.name);
        sceneManager.stageLevel = Convert.ToInt32(this.gameObject.name);
        stageSelect.PrevTargetUI = stageSelect.targetUI;
        stageSelect.IsTargetUIChoose = true;
    }


    public void StageClick()
    {
        sceneManager.stageName = this.gameObject.name;
        sceneManager.IsRestart = false;

        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        audioSource.Play();

        SceneManager.LoadScene("game");

    }
}
