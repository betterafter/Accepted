using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrow : MonoBehaviour
{
    private Stage s;
    private Button btn;
    private Transform btnObj;
    private Map_Editor mapEditor;

    private string btnname, dirname, dirbuttonName;



    private void Awake()
    {
        btnname = gameObject.tag;
        dirname = btnname.Replace("step", "obj");
        dirbuttonName = btnname.Replace("step", "");

        mapEditor = Camera.main.GetComponent<Map_Editor>();
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {

        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (dirname == collision.gameObject.tag)
        {
            s.currStepCnt++;

            if (gameObject.tag.Contains("left")) s.IsLeftActive++;
            else if (gameObject.tag.Contains("right")) s.IsRightActive++;
            else if (gameObject.tag.Contains("up")) s.IsUpActive++;
            else s.IsDownActive++;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (dirname == collision.gameObject.tag)
        {
            //btnObj.gameObject.SetActive(false);

            s.currStepCnt--;

            if (gameObject.tag.Contains("left")) s.IsLeftActive--;
            else if (gameObject.tag.Contains("right")) s.IsRightActive--;
            else if (gameObject.tag.Contains("up")) s.IsUpActive--;
            else s.IsDownActive--;
        }
    }
}
