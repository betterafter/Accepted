using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection : MonoBehaviour
{
    public GameObject TargetObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            TargetObject = collision.gameObject;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(249 / 255f, 63 / 255f, 0 / 255f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == TargetObject)
        {
            TargetObject = null;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(83 / 255f, 150 / 255f, 255 / 255f);
        }
    }

    private void Update()
    {
        if(TargetObject != null)
        {

        }
    }
}
