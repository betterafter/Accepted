using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollInformation : MonoBehaviour
{
    public GameObject ImgRect;
    public Button LeftButton, RightButton;
    int leftClick, rightClick;
    Vector3 CurrPosition, targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        leftClick = 2; rightClick = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 curr = ImgRect.transform.position;

        if (leftClick == 1 && rightClick == 0)
        {
            targetPosition = new Vector3(CurrPosition.x - 10, CurrPosition.y, CurrPosition.z);
            ImgRect.transform.position = Vector3.Lerp(ImgRect.transform.position, targetPosition, 0.2f);
            if (ImgRect.transform.position.x - targetPosition.x < 0.001) {
                ImgRect.transform.position = new Vector3(Mathf.Round(ImgRect.transform.position.x), ImgRect.transform.position.y, ImgRect.transform.position.z);
                leftClick = 2; rightClick = 2;
                RightButton.enabled = true; 
            }
        }
        else if (leftClick == 0 && rightClick == 1)
        {
            targetPosition = new Vector3(CurrPosition.x + 10, CurrPosition.y, CurrPosition.z);
            ImgRect.transform.position = Vector3.Lerp(ImgRect.transform.position, targetPosition, 0.2f);
            if (targetPosition.x - ImgRect.transform.position.x < 0.001) {
                ImgRect.transform.position = new Vector3(Mathf.Round(ImgRect.transform.position.x), ImgRect.transform.position.y, ImgRect.transform.position.z);
                leftClick = 2; rightClick = 2;
                LeftButton.enabled = true;
            }
        }
        else if(leftClick == 2 && rightClick == 2)
        {
            CurrPosition = ImgRect.transform.position;
        }
    }

    public void LeftClick()
    {
        leftClick = 1;
        rightClick = 0;
        RightButton.enabled = false;
    }
    
    public void RightClick()
    {
        leftClick = 0;
        rightClick = 1;
        LeftButton.enabled = false;
    }

}
