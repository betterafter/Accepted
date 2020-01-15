using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class stageLevelSelect : MonoBehaviour
{
    Vector3 DisablePosition, EnablePosition;
    StageSelect StageSelect;

    private void Start()
    {
        StageSelect = Camera.main.GetComponent<StageSelect>();

        DisablePosition = new Vector3(-500, 0);
        EnablePosition = new Vector3(0, 0);

    }

    private void Update()
    {
        EnableStage();
    }


    void EnableStage()
    {
        Vector3 TargetPosition;
        int i = Convert.ToInt32(gameObject.name);

        if (StageSelect.targetUI == i) TargetPosition = EnablePosition;
        else TargetPosition = DisablePosition;

        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, TargetPosition, 0.2f);
        if (Vector3.Distance(gameObject.GetComponent<RectTransform>().anchoredPosition, TargetPosition) < 1)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = TargetPosition;
        }

    }
}
