using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectAnimation : MonoBehaviour
{
    Vector3 OriginScale;

    private void Start()
    {
        OriginScale = this.gameObject.GetComponent<RectTransform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Image>().color.a > 0)
        {
            gameObject.GetComponent<RectTransform>().localScale
                = new Vector3(gameObject.GetComponent<RectTransform>().localScale.x + 0.005f, gameObject.GetComponent<RectTransform>().localScale.y + 0.005f);
        }
        Color color = gameObject.GetComponent<Image>().color;
        color.a -= 0.02f;
        gameObject.GetComponent<Image>().color = color;

        if (gameObject.GetComponent<Image>().color.a <= 0)
        {
            color.a = 1;
            gameObject.GetComponent<Image>().color = color;
            gameObject.GetComponent<RectTransform>().localScale = OriginScale;
        }
    }
}
