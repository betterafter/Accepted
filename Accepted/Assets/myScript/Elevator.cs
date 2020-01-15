using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    StageSelect stageSelect;
    Vector3 targetPosition, CurrPosition;
    public bool ReadyToMove = true;

    private void Start()
    {
        stageSelect = Camera.main.GetComponent<StageSelect>();
        CurrPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if (stageSelect.leftClick == 1 && stageSelect.rightClick == 0 && ReadyToMove)
        {
            targetPosition = new Vector3(CurrPosition.x, CurrPosition.y - 1000);
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, targetPosition, 0.2f);
            if (Vector3.Distance(gameObject.GetComponent<RectTransform>().anchoredPosition, targetPosition) < 1)
            {
                ReadyToMove = false;
                if (gameObject.GetComponent<RectTransform>().anchoredPosition.y < 0)
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CurrPosition.x, (int)(gameObject.GetComponent<RectTransform>().anchoredPosition.y - 1));
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CurrPosition.x, (int)(gameObject.GetComponent<RectTransform>().anchoredPosition.y));

            }
        }

        else if (stageSelect.leftClick == 0 && stageSelect.rightClick == 1 && ReadyToMove)
        {
            targetPosition = new Vector3(CurrPosition.x, CurrPosition.y + 1000);
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, targetPosition, 0.2f);
            if (Vector3.Distance(gameObject.GetComponent<RectTransform>().anchoredPosition, targetPosition) < 1)
            {
                ReadyToMove = false;
                if (gameObject.GetComponent<RectTransform>().anchoredPosition.y < 0)
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CurrPosition.x, (int)(gameObject.GetComponent<RectTransform>().anchoredPosition.y));
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CurrPosition.x, (int)(gameObject.GetComponent<RectTransform>().anchoredPosition.y + 1));

            }
        }

        else if (stageSelect.leftClick == 2 && stageSelect.rightClick == 2) { ReadyToMove = true; CurrPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition; }
        }

}
