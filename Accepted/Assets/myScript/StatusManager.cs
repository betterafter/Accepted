using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour
{
    public int RotateAngle;
    public int CloneNum;

    public bool ReadyToCloneSpawn;
    public bool ReadyToRobotOperation;


    private void Start()
    {
        ReadyToRobotOperation = false;
        ReadyToCloneSpawn = false;
        RotateAngle = 0;
    }

    public void Click_yes()
    {
        sceneManager scene = GameObject.Find("GameManager").GetComponent<sceneManager>();
        int stageLevel = scene.stageLevel;
        SceneManager.LoadScene(stageLevel.ToString());
    }

    public void Click_no()
    {
        GameObject Quit = GameObject.Find("quit");
        Quit.transform.Find("quitUI").gameObject.SetActive(false);
    }

    private void Update()
    {
        if ((int)this.transform.eulerAngles.z != 0)
        {
            if ((int)(this.transform.eulerAngles.z % 270) == 0)
            {
                RotateAngle = 270;
            }
            else if ((int)(this.transform.eulerAngles.z % 180) == 0)
            {
                RotateAngle = 180;
            }
            else if ((int)(this.transform.eulerAngles.z % 90) == 0)
            {
                RotateAngle = 90;
            }
            else
            {
                RotateAngle = 0;
            }
        }
        else
        {
            RotateAngle = 0;
        }


        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GameObject Quit = GameObject.Find("quit");
                Quit.transform.Find("quitUI").gameObject.SetActive(true);
            }
        }
    }

   
}
