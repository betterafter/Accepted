using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    private StatusManager statusManager;

    private void Start()
    {
        statusManager = Camera.main.GetComponent<StatusManager>();
        //StartCoroutine("RotateCheck");

    }

    private void Update()
    {
        if (this.gameObject.CompareTag("accepted"))
        {
            if ((int)Camera.main.transform.eulerAngles.z != 0)
            {
                if ((int)(Camera.main.transform.eulerAngles.z % 270) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 270);
                }
                else if ((int)(Camera.main.transform.eulerAngles.z % 180) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if ((int)(Camera.main.transform.eulerAngles.z % 90) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else if (this.gameObject.name == "gamebutton")
        {
            if ((int)Camera.main.transform.eulerAngles.z != 0)
            {
                if ((int)(Camera.main.transform.eulerAngles.z % 270) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if ((int)(Camera.main.transform.eulerAngles.z % 180) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if ((int)(Camera.main.transform.eulerAngles.z % 90) == 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }



}
