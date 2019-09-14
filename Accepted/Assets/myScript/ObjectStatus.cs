using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    private StatusManager statusManager;

    private Transform UndoPosition;
    private sceneManager sceneManager;
    private Stack<Vector3> positionStack = new Stack<Vector3>();

    private Vector3 GameObjectPosition;
    private Vector3 PrevGameObjectPosition;

    private PlayerController playerController;



    private void Start()
    {
        statusManager = Camera.main.GetComponent<StatusManager>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();

        PrevGameObjectPosition = gameObject.transform.position;
        GameObjectPosition = gameObject.transform.position;

        StartCoroutine("PositionStack");
        StartCoroutine("PushPositionStack");
        StartCoroutine("CloneSpawnStopFirst");

        playerController = GameObject.FindWithTag("player").GetComponent<PlayerController>();
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

    private void FixedUpdate()
    {
        sceneManager.IsUndo = false;
        sceneManager.IsLastClickedButton_Undo = false;


    }

    private IEnumerator PositionStack()
    {
        while (true)
        {
            if (positionStack.Count > 0)
            {
                if (sceneManager.IsUndo == true)
                {
                    gameObject.transform.position = positionStack.Peek();
                    PrevGameObjectPosition = gameObject.transform.position;
                    positionStack.Pop();

                    //if(gameObject.tag == "player" && positionStack.Count > 0) Debug.Log(positionStack.Peek());
                }
            }
            yield return new WaitWhile(() => sceneManager.IsUndo == true);
        }
    }

    private IEnumerator PushPositionStack()
    {
        while (true)
        {

            if (sceneManager.IsLastClickButton_Move == false && sceneManager.IsUndo == false && sceneManager.dir == 0)
            {
                positionStack.Push(PrevGameObjectPosition);
                PrevGameObjectPosition = gameObject.transform.position;

                //if (gameObject.tag == "player" && positionStack.Count > 0) Debug.Log(positionStack.Peek());
            }

            yield return new WaitWhile(() => sceneManager.IsLastClickButton_Move == false);
        }
    }

    private IEnumerator CloneSpawnStopFirst()
    {
        while(true)
        {
            if (gameObject.CompareTag("player"))
            {
                if (sceneManager.dir == 0 && gameObject.GetComponent<PlayerController>().IsClone == 1)
                {
                    gameObject.GetComponent<PlayerController>().enabled = true;
                    break;
                }
            }

            yield return null;
        }
    }
}
