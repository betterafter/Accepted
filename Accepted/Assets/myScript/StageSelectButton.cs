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
    GameObject nextScene;
    Camera cam;


    private void Start()
    {
        nextScene = GameObject.Find("NextSceneCanvas").transform.GetChild(0).gameObject;
        cam = Camera.main;


        stageSelect = Camera.main.GetComponent<StageSelect>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();

        if (gameObject.CompareTag("stage"))
            gameObject.GetComponent<Button>().onClick.AddListener(delegate () { gameObject.GetComponent<StageSelectButton>().StageClick(); });
    }


    public void FloorClick()
    {
        stageSelect.targetY = this.gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition.y;

        stageSelect.targetUI = Convert.ToInt32(this.gameObject.name);
        sceneManager.stageLevel = Convert.ToInt32(this.gameObject.name);
        stageSelect.PrevTargetUI = stageSelect.targetUI;
        stageSelect.IsTargetUIChoose = true;

        stageSelect.effect[stageSelect.targetUI].SetActive(true);
    }


    public void StageClick()
    {
        sceneManager.stageName = this.gameObject.name;
        sceneManager.IsRestart = false;

        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        audioSource.Play();

        cam.GetComponent<CameraResolution>().color = Color.black;

        Color color = nextScene.GetComponent<Image>().color;
        color = Color.black;
        color.a = 0;
        nextScene.GetComponent<Image>().color = color;

        stageSelect.ReadyToNextScene = true;
        StartCoroutine("ChangeScene");
    }

    IEnumerator ChangeScene()
    {
        if (nextScene.GetComponent<Image>().color.a < 1)
        {
            Color color = nextScene.GetComponent<Image>().color;

            while (true)
            {
                color.a += 0.02f;

                //Debug.Log(color);

                nextScene.GetComponent<Image>().color = color;
                if(nextScene.GetComponent<Image>().color.a >= 1) SceneManager.LoadScene("game");

                yield return null;
            }
        }
    }

}
