using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCollision : MonoBehaviour
{
    private Stage s;

    private void Start()
    {
        s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s.currStepCnt++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s.currStepCnt--;
        }
    }
}
