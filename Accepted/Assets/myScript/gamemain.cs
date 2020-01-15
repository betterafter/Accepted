﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class gamemain : MonoBehaviour
{
    public AudioSource audioSource;
    public Text TapToStart;
    private Vector3 MousePosition;

    public GameObject block, step, Canvas1, Canvas2, TouchText;
    public Text AcceptedText, text1, text2, text3, text4, mainText;
    public Animator anime;

    Vector3 targetPosition = new Vector3(0, 110, 0);

    bool isReadyToStart = false;

    private void Update()
    {
        if (!GameObject.Find("GameManager").GetComponent<sceneManager>().isBack)
        {
            float y1 = text1.GetComponent<RectTransform>().anchoredPosition.y, y2 = text2.GetComponent<RectTransform>().anchoredPosition.y;
            float x1 = text3.GetComponent<RectTransform>().anchoredPosition.x, x2 = text4.GetComponent<RectTransform>().anchoredPosition.x;


            if (text1.GetComponent<RectTransform>().anchoredPosition.y < 110)
            {
                y1 += 0.5f;
                text1.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, y1);
            }
            if(text2.GetComponent<RectTransform>().anchoredPosition.y > 110)
            {
                y2 -= 0.5f;
                text2.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, y2);
            }
            if (text3.GetComponent<RectTransform>().anchoredPosition.x > 0)
            {
                x1 -= 0.5f;
                text3.GetComponent<RectTransform>().anchoredPosition = new Vector3(x1, 110);
            }
            if (text4.GetComponent<RectTransform>().anchoredPosition.x < 0)
            {
                x2 += 0.5f;
                text4.GetComponent<RectTransform>().anchoredPosition = new Vector3(x2, 110);
            }

            if(Vector3.Distance(text1.GetComponent<RectTransform>().anchoredPosition, targetPosition) <= 0.01f &&
                Vector3.Distance(text2.GetComponent<RectTransform>().anchoredPosition, targetPosition) <= 0.01f &&
                Vector3.Distance(text3.GetComponent<RectTransform>().anchoredPosition, targetPosition) <= 0.01f &&
                Vector3.Distance(text4.GetComponent<RectTransform>().anchoredPosition, targetPosition) <= 0.01f)

            {
                anime.enabled = true; isReadyToStart = true; Canvas1.SetActive(true); TouchText.SetActive(true);
            }
        }
        else
        {
            text1.enabled = false; text2.enabled = false; text3.enabled = false; text4.enabled = false;
            anime.enabled = true; isReadyToStart = true; Canvas1.SetActive(true); TouchText.SetActive(true);
        }
    }


    //private void show()
    //{
    //    StartCoroutine("Accepted");
    //}

    //private IEnumerator Accepted()
    //{
    //    Color TextColor = AcceptedText.color;
    //    while (TextColor.a <= 1.0f)
    //    {
    //        TextColor.a += 0.05f;
    //        AcceptedText.color = TextColor;
    //        yield return new WaitForSeconds(0.05f);
    //    }

    //    TouchText.SetActive(true);
    //    isReadyToStart = true;
    //}


    private void Start()
    {
        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        StartCoroutine("Blink");
        StartCoroutine("ScreenTapToStart");
        StartCoroutine("mainTextOpacity");
    }

    public void OnStartClick()
    {
        audioSource.Play();
        SceneManager.LoadScene("stageSelect");
    }

    public void OnMapMakeClick()
    {
        audioSource.Play();
        SceneManager.LoadScene("MapCreater");
    }

    public void OnGoogleLoginClick()
    {
        audioSource.Play();
    }

    public void OnOptionClick()
    {
        audioSource.Play();
    }

    public void OnQuitClick()
    {
        audioSource.Play();
        Application.Quit();
    }

    IEnumerator Blink()
    {
        Color TextColor = TapToStart.color;

  
        while (true)
        {
            if (isReadyToStart)
            {
                while (TextColor.a <= 1.0f)
                {
                    TextColor.a += 0.1f;
                    TapToStart.color = TextColor;
                    yield return new WaitForSeconds(0.05f);
                }

                while (TextColor.a >= 0.0f)
                {
                    TextColor.a -= 0.1f;
                    TapToStart.color = TextColor;
                    yield return new WaitForSeconds(0.05f);
                }

            }
            yield return new WaitForSeconds(0.1f);
        
        }
        
    }


    IEnumerator mainTextOpacity()
    {
        Color TextColor = mainText.color;

        while (true)
        {
            while (TextColor.a <= 1.0f)
            {
                TextColor.a += 0.1f;
                mainText.color = TextColor;
                yield return new WaitForSeconds(0.05f);
            }

        yield return new WaitForSeconds(0.1f);

        }

    }

    private IEnumerator ScreenTapToStart()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject() && isReadyToStart) OnStartClick();
                }
                yield return null;
            }
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && isReadyToStart)
                        {
                            OnStartClick();
                        }
                    }
                }
                yield return null;
            }
        }

    }

}
