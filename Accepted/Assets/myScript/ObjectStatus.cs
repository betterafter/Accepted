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
    public bool IsReadyToMove;
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

        playerController = GameObject.FindWithTag("player").GetComponent<PlayerController>();
        map_Editor = Camera.main.GetComponent<Map_Editor>();

        if(this.gameObject.tag.Contains("player"))
        {
            Upbtn = map_Editor.Upbtn;
            Downbtn = map_Editor.Downbtn;
            Leftbtn = map_Editor.Leftbtn;
            Rightbtn = map_Editor.Rightbtn;

            Upbtn.onClick.AddListener(delegate () { gameObject.GetComponent<PlayerController>().Move(Upbtn); });
            Downbtn.onClick.AddListener(delegate () { gameObject.GetComponent<PlayerController>().Move(Downbtn); });
            Leftbtn.onClick.AddListener(delegate () { gameObject.GetComponent<PlayerController>().Move(Leftbtn); });
            Rightbtn.onClick.AddListener(delegate () { gameObject.GetComponent<PlayerController>().Move(Rightbtn); });
        }
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
                if (PrevGameObjectPosition != gameObject.transform.position)
                {
                    positionStack.Push(PrevGameObjectPosition);
                    PrevGameObjectPosition = gameObject.transform.position;
                }

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


    /// clone이 파괴되면 visible, playercontroller, collision을 꺼서 스택에 저장. 즉 매 프레임마다 Destroy를 정의하는 특정 Vector 혹은 배열을 스택에 저장. 
    /// Undo를 클릭할 때 이전 프레임에 Destroy상태인지 아닌지에 따라 킬지 말지 정함.
    /// 
    /// 플레이어가 못 미는 블럭을 만들어서 로봇만 밀 수 있게 만들자.
    /// 
    /// 로봇은 배터리의 개수만큼 움직일 수 있다. undo를 누르면 배터리가 1개씩 차도록 하자.
    /// 
    /// 로봇은 비행모드가 존재한다. 비행기모드를 켜서 비행모드를 실행할 수 있다.
    /// 
    /// 클론은 스폰위치에 닿으면 파괴된다. (위의 Destroy상태) 
}
