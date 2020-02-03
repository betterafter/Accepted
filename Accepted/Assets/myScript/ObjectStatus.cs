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

    public string Currtag;

    Vector3 prevPos;

    public bool[] testbool = new bool[4];

    private void Start()
    {
        prevPos = this.gameObject.transform.position;

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

        if (this.gameObject.tag.Contains("bar")) Currtag = this.gameObject.tag;
    }



    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, PrevGameObjectPosition) > 0.01f)
        {
            sceneManager.isChanged = true;
        }

        if (sceneManager.rotatebool[2])
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 270);
            if (Currtag == "hobar") this.gameObject.tag = "verbar";
            else if (Currtag == "verbar") this.gameObject.tag = "hobar";
        }
        else if (sceneManager.rotatebool[1])
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 180);
            if (Currtag == "hobar") this.gameObject.tag = "hobar";
            else if (Currtag == "verbar") this.gameObject.tag = "verbar";
        }
        else if (sceneManager.rotatebool[0])
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 90);
            if (Currtag == "hobar") this.gameObject.tag = "verbar";
            else if (Currtag == "verbar") this.gameObject.tag = "hobar";
        }
        else 
        { 
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Currtag == "hobar") this.gameObject.tag = "hobar";
            else if (Currtag == "verbar") this.gameObject.tag = "verbar";
        }
        

        if (Vector3.Distance(prevPos, gameObject.transform.position) > 0)
        {
            sceneManager.minimap[(int)prevPos.y + 5, (int)prevPos.x + 7] = null;

        }

        if (sceneManager.dir == 0 && !this.gameObject.tag.Contains("step") && !this.gameObject.tag.Contains("accepted") && !sceneManager.IsUndo)
        {

            sceneManager.minimap[(int)(this.gameObject.transform.position.y + 5), (int)(this.gameObject.transform.position.x + 7)]
                    = this.gameObject;
            prevPos = this.gameObject.transform.position;
        }
    }

    private void FixedUpdate()
    {
        sceneManager.IsUndo = false;
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
