using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    sceneManager sceneManager;
    Map_Editor Map_Editor;
    Stack<bool> pointEnabled = new Stack<bool>();

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("player") && collision.gameObject.GetComponent<PlayerController>().IsClone == 0)
    //    {
    //        GameObject Manager = GameObject.Find("GameManager");

    //        Manager.GetComponent<sceneManager>().currPoint++;
    //        this.gameObject.SetActive(false);
    //    }
    //}



    void Start()
    {
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        Map_Editor = Camera.main.GetComponent<Map_Editor>();
        StartCoroutine("PositionStack");
        StartCoroutine("PushPositionStack");

    }


    private IEnumerator PositionStack()
    {
        while (true)
        {
            if (pointEnabled.Count > 0)
            {
                if (sceneManager.IsUndo)
                {
                    Debug.Log("Undo");
                    if (pointEnabled.Peek())
                    {
                        gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    pointEnabled.Pop();
                }
            }
            yield return new WaitWhile(() => sceneManager.IsUndo);
        }
    }

    private IEnumerator PushPositionStack()
    {
        while (true)
        {
            if (!sceneManager.IsLastClickButton_Move && sceneManager.isChanged)
            {
                Debug.Log(pointEnabled.Count);
                if(Vector3.Distance(Map_Editor.player.transform.position, this.gameObject.transform.position) < 0.0001f)
                {
                    if (gameObject.GetComponent<SpriteRenderer>().enabled)
                    {
                        pointEnabled.Push(true);
                        gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        pointEnabled.Push(false);
                    }
                }
                else
                {
                    pointEnabled.Push(false);
                }
            }

            yield return new WaitWhile(() => !sceneManager.IsLastClickButton_Move);
        }
    }
}
