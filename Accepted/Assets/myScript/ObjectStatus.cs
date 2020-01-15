using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStatus : MonoBehaviour
{
    private StatusManager statusManager;

    private Transform UndoPosition;
    private sceneManager sceneManager;
    private Stack<Vector3> positionStack = new Stack<Vector3>();

    private Vector3 GameObjectPosition;
    private Vector3 PrevGameObjectPosition;
    public bool IsReadyToMove, afterUndo = false;
    //public bool LeaveSpawner;

    private PlayerController playerController;
    private Map_Editor map_Editor;
    private Button Upbtn, Downbtn, Leftbtn, Rightbtn;

    private void Start()
    {
        int cx = Mathf.RoundToInt(this.gameObject.transform.position.x);
        int cy = Mathf.RoundToInt(this.gameObject.transform.position.y);

        this.gameObject.transform.position = new Vector2(cx, cy);

        statusManager = Camera.main.GetComponent<StatusManager>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        IsReadyToMove = true;

        //LeaveSpawner = false;

        PrevGameObjectPosition = gameObject.transform.position;
        GameObjectPosition = gameObject.transform.position;

        StartCoroutine("PositionStack");
        StartCoroutine("PushPositionStack");
        StartCoroutine("CloneSpawnStopFirst");
        StartCoroutine("AccessRobot");
    }



    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, PrevGameObjectPosition) > 0.01f)
        {
            sceneManager.isChanged = true;
        }

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

        if(sceneManager.dir == 0 && !this.gameObject.tag.Contains("step") && !this.gameObject.tag.Contains("accepted") && !sceneManager.IsUndo)
        {
            sceneManager.minimap[(int)(this.gameObject.transform.position.y + 5), (int)(this.gameObject.transform.position.x + 7)]
                    = this.gameObject;

        }
    }

    private void FixedUpdate()
    {
        sceneManager.IsUndo = false;
        //sceneManager.IsLastClickedButton_Undo = false;
       
    }

    private IEnumerator PositionStack()
    {
        while (true)
        {
            if (positionStack.Count > 0)
            {
                if (sceneManager.IsUndo)
                {
                    gameObject.transform.position = positionStack.Peek();
                    PrevGameObjectPosition = gameObject.transform.position;
                    positionStack.Pop();

                    //afterUndo = true;
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
                positionStack.Push(PrevGameObjectPosition);
                PrevGameObjectPosition = gameObject.transform.position;
            }

            yield return new WaitWhile(() => !sceneManager.IsLastClickButton_Move);
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

    private IEnumerator AccessRobot()
    {
        if (gameObject.CompareTag("robotplayer"))
        {
            while (true)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable || !statusManager.ReadyToRobotOperation)
                {
                    gameObject.GetComponent<PlayerController>().enabled = false;
                }
                else if(Application.internetReachability != NetworkReachability.NotReachable && statusManager.ReadyToRobotOperation)
                {
                    gameObject.GetComponent<PlayerController>().enabled = true;
                }
                yield return null;
            }
        }

        if (gameObject.CompareTag("player"))
        {
            while (true)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable || !statusManager.ReadyToRobotOperation)
                {
                    if (this.gameObject.GetComponent<PlayerController>().IsClone == 0)
                    {
                        gameObject.GetComponent<PlayerController>().enabled = true;
                    }
                }
                else if (Application.internetReachability != NetworkReachability.NotReachable && statusManager.ReadyToRobotOperation)
                {
                    if (this.gameObject.GetComponent<PlayerController>().IsClone == 0)
                    {
                        gameObject.GetComponent<PlayerController>().enabled = false;
                    }
                }
                yield return null;
            }
        }

    }

}
