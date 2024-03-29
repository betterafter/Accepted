﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;



public class StageSelect : MonoBehaviour
{

   //public sceneManager s;

    AudioSource audioSource;

    GameObject[] FloorInCanvas = new GameObject[8];
    GameObject[] FloorNameInCanvas = new GameObject[8];
    GameObject[] Stage = new GameObject[8];
   
    public GameObject[] effect = new GameObject[8];
    public GameObject[] CanvasChild = new GameObject[8];

    GameObject[] Canvas = new GameObject[8];
    Vector3[] FloorOriginVectorInCanvas = new Vector3[8];
    Button[] FloorButton = new Button[8];

    public GameObject left, right, nextScene;
    public Button LeftButton, RightButton;
    public int leftClick, rightClick;

    Elevator elevator;

    public float targetX = 200, targetY;
    public int targetUI, PrevTargetUI;
    public bool IsTargetUIChoose = false;

    private Vector3 MousePosition;
    public Vector3[] CurrPosition = new Vector3[8];

    public Sprite EnableSprite, DisableSprite;
    public bool ReadyToNextScene = false;

    public GameObject Light;

    //Vector3[] targetPosition = new Vector3[8];
   


    private void Start()
    {

        StartCoroutine("Reset");
        StartCoroutine("ThisScene");

        leftClick = 2; rightClick = 2;
        GameObject StageCanvas = GameObject.Find("Stage");

        for (int i = 1; i <= 7; i++)
        {
            Stage[i] = StageCanvas.transform.GetChild(i - 1).gameObject;


            Canvas[i] = GameObject.Find("CanvasPool").transform.GetChild(i - 1).gameObject;
            // canvas -> 2
            CanvasChild[i] = Canvas[i].transform.GetChild(0).gameObject;
            // canvas -> 2 -> 2(button)
            FloorInCanvas[i] = CanvasChild[i].gameObject.transform.GetChild(1).gameObject;
            // canvas -> 2 -> 2(button) -> button(component)
            FloorButton[i] = FloorInCanvas[i].GetComponent<Button>();
            // canvas -> 2 -> text
            FloorNameInCanvas[i] = CanvasChild[i].transform.GetChild(2).gameObject;
            // canvas -> 2 -> position
            if(i < 5)
                FloorOriginVectorInCanvas[i] = CanvasChild[i].GetComponent<RectTransform>().anchoredPosition;
            else if(i >= 5)
                FloorOriginVectorInCanvas[i] = new Vector3(CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.x, CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y + 1000);

            effect[i] = CanvasChild[i].gameObject.transform.GetChild(0).gameObject;
            GameObject o = FloorInCanvas[i];

            FloorButton[i].onClick.AddListener(delegate() { o.GetComponent<StageSelectButton>().FloorClick(); });
            CurrPosition[i] = CanvasChild[i].GetComponent<RectTransform>().anchoredPosition;
            //Debug.Log(CanvasChild[i].name);
        }
    }

    void Func1(int i)
    {

        FloorNameInCanvas[i].SetActive(false);
        if (targetUI == i)
        {
            Canvas[i].GetComponent<Canvas>().sortingOrder = 1;
        }

        if ((int)targetY != (int)CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y)
        {
            if (FloorOriginVectorInCanvas[i].y > targetY)
            {
                CanvasChild[i].GetComponent<RectTransform>().anchoredPosition
                     = new Vector3(FloorOriginVectorInCanvas[i].x, CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y - 50);
            }
            else
            {
                CanvasChild[i].GetComponent<RectTransform>().anchoredPosition
                    = new Vector3(FloorOriginVectorInCanvas[i].x, CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y + 50);
            }
        }
    }

    void Func2(int i)
    {
        //Debug.Log("Func2");
        Canvas[i].GetComponent<Canvas>().sortingOrder = 0;
        FloorNameInCanvas[i].SetActive(true);

        if ((int)FloorOriginVectorInCanvas[i].y != (int)CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y)
        {
            if (FloorOriginVectorInCanvas[i].y > (int)CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y)
            {
                CanvasChild[i].GetComponent<RectTransform>().anchoredPosition
                     = new Vector3(FloorOriginVectorInCanvas[i].x, CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y + 50);
            }
            else
            {
                CanvasChild[i].GetComponent<RectTransform>().anchoredPosition
                    = new Vector3(FloorOriginVectorInCanvas[i].x, CanvasChild[i].GetComponent<RectTransform>().anchoredPosition.y - 50);
            }
        }
    }



    private void Update()
    {
        Vector3 vector = Light.GetComponent<RectTransform>().anchoredPosition;
        Light.GetComponent<RectTransform>().anchoredPosition = new Vector3(vector.x, vector.y - 5f);

        if(Light.GetComponent<RectTransform>().anchoredPosition.y <= -1000)
        {
            Light.GetComponent<RectTransform>().anchoredPosition = new Vector3(vector.x, 1000);
        }

        if (leftClick == 2 && rightClick == 2 && CanvasChild[1].GetComponent<RectTransform>().anchoredPosition.y <= 300)
        {
            if (IsTargetUIChoose)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Func1(i);
                }
            }

            else if (!IsTargetUIChoose)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Func2(i);
                }
            }
        }

        if (leftClick == 2 && rightClick == 2 && CanvasChild[7].GetComponent<RectTransform>().anchoredPosition.y >= -200)
        {
            if (IsTargetUIChoose)
            {
                for (int i = 5; i <= 7; i++)
                {
                    Func1(i);
                }
            }

            else if (!IsTargetUIChoose)
            {
                for (int i = 5; i <= 7; i++)
                {
                    Func2(i);
                }
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            if (CanvasChild[i].GetComponent<Elevator>().ReadyToMove) break;
            if(i == 7) { leftClick = 2; rightClick = 2; }
        }

        if (leftClick != 2 || rightClick != 2)
        {
            right.SetActive(false); left.SetActive(false);
        }
        else if (leftClick == 2 && rightClick == 2)
        {
            if (CanvasChild[1].GetComponent<RectTransform>().anchoredPosition.y <= 300)
            {
                right.SetActive(true); left.SetActive(false);
            }
            else if (CanvasChild[1].GetComponent<RectTransform>().anchoredPosition.y <= 1300)
            {
                right.SetActive(false); left.SetActive(true);
            }
        }

       
    }


    IEnumerator Reset()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))

                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        IsTargetUIChoose = false;
                        targetUI = 0;

                        for(int i = 1; i <= 7; i++)
                        {
                            effect[i].SetActive(false);
                        }
                    }
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
                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        {
                            Debug.Log("Touch Screen");

                            IsTargetUIChoose = false;
                            targetUI = 0;

                            for (int i = 1; i <= 7; i++)
                            {
                                effect[i].SetActive(false);
                            }
                        }
                    }
                }
                yield return null;
            }
        }
    }

    public IEnumerator ThisScene()
    {
        if (!ReadyToNextScene)
        {
            if (nextScene.GetComponent<Image>().color.a >= 1)
            {
                while (true)
                {
                    Color color = nextScene.GetComponent<Image>().color;
                    color.a -= 0.02f;

                    nextScene.GetComponent<Image>().color = color;

                    yield return null;
                }
            }
        }
    }





    public void LeftClick()
    {
        leftClick = 1;
        rightClick = 0;
        IsTargetUIChoose = false;
        targetUI = 0;

        for (int i = 1; i <= 7; i++)
        {
            effect[i].SetActive(false);
        }

    }

    public void RightClick()
    {
        leftClick = 0;
        rightClick = 1;
        IsTargetUIChoose = false;
        targetUI = 0;

        for (int i = 1; i <= 7; i++)
        {
            effect[i].SetActive(false);
        }
    }


}
