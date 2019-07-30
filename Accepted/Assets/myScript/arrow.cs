using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrow : MonoBehaviour
{
    private Stage s;
    private Button upbtn, downbtn, leftbtn, rightbtn, btn;
    private string btnname;
    private bool IsBtnActive;


    private void Start()
    {
        btnname = gameObject.tag;
        btnname = btnname.Replace("step", "");
        IsBtnActive = false;

        btn = GameObject.Find(btnname).GetComponent<Button>();

        StartCoroutine("BtnIsActive");
    }


    IEnumerator BtnIsActive()
    {
        while (true)
        {
            if (IsBtnActive == false)
            {
                btn.gameObject.SetActive(false);
            }
            else if (IsBtnActive == true)
            {
                btn.gameObject.SetActive(true);
            }

            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (collision.gameObject.tag.Contains(btnname))
        {
            IsBtnActive = true;
            s.currStepCnt++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (collision.gameObject.tag.Contains(btnname))
        {
            IsBtnActive = false;
            s.currStepCnt--;
        }
    }
}
