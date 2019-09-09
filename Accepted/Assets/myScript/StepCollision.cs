using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCollision : MonoBehaviour
{
    private Stage s;

    private void Start()
    {
     
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            s.currStepCnt++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            s.currStepCnt--;
        }
    }
}
