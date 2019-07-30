using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCollision : MonoBehaviour
{
    private Stage s;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s.currStepCnt++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();

        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s.currStepCnt--;
        }
    }
}
