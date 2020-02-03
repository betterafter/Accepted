using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("player") && collision.gameObject.GetComponent<PlayerController>().IsClone == 0)
        {
            GameObject Manager = GameObject.Find("GameManager");

            Manager.GetComponent<sceneManager>().currPoint++;
            this.gameObject.SetActive(false);
        }
    }
}
