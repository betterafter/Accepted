using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private Vector3 rotateAngle;

    private void Start()
    {
        rotateAngle = new Vector3(0, 0, 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("rotateobj"))
        {
            Camera.main.gameObject.transform.eulerAngles += rotateAngle; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("rotateobj"))
        {
            Camera.main.gameObject.transform.eulerAngles -= rotateAngle;
        }
    }
}
