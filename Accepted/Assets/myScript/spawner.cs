using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawner : MonoBehaviour
{
    private GameObject Spawn;
    private GameObject Robot;
    private GameObject Player;

    private bool SpawnBlocked;

    private Vector3 MousePosition;
    private Vector3 SpawnPosition;

    private StatusManager statusManager;
    private sceneManager sceneManager;
    private Map_Editor map_Editor;

    void Start()
    {
        SpawnBlocked = false;
        statusManager = Camera.main.GetComponent<StatusManager>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        map_Editor = Camera.main.GetComponent<Map_Editor>();

        StartCoroutine("TouchToCopy");
    }




    private IEnumerator TouchToCopy()
    {
        while (true)
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                if (Input.GetMouseButtonDown(0))

                {
                    MousePosition = Input.mousePosition;
                    MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

                    RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, 15f);

                    if (hit)
                    {
                        if (hit.collider != null)

                        {
                            GameObject touchObject = hit.collider.gameObject;
                            Debug.Log(touchObject);
                            Debug.Log(statusManager.ReadyToCloneSpawn);
                            if (touchObject.CompareTag("clonespawn") && statusManager.ReadyToCloneSpawn)
                            {
                                Debug.Log("!");
                                SpawnPosition = touchObject.transform.position;
                                if (SpawnBlocked == false)
                                {
                                    GameObject o = Resources.Load("Prefabs/obj/player") as GameObject;
                                    GameObject b = Instantiate(o, SpawnPosition, Quaternion.identity);
                                    b.GetComponent<SpriteRenderer>().color = new Color(178 / 255f, 178 / 255f, 178 / 255f);
                                    b.tag = "player";
                                    b.GetComponent<PlayerController>().IsClone = 1;

                                    PlayerController playerController = b.GetComponent<PlayerController>();
                                    playerController.enabled = true;

                                    map_Editor.Upbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Upbtn); });
                                    map_Editor.Downbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Downbtn); });
                                    map_Editor.Leftbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Leftbtn); });
                                    map_Editor.Rightbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Rightbtn); });
                                }
                            }
                        }
                    }
                }
            }

            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                        if (hit)
                        {
                            if (hit.collider != null)

                            {
                                GameObject touchObject = hit.collider.gameObject;
                                Debug.Log(touchObject);
                                Debug.Log(statusManager.ReadyToCloneSpawn);
                                if (touchObject.CompareTag("clonespawn") && statusManager.ReadyToCloneSpawn)
                                {
                                    Debug.Log("!");
                                    SpawnPosition = touchObject.transform.position;
                                    if (SpawnBlocked == false)
                                    {
                                        GameObject o = Resources.Load("Prefabs/obj/player") as GameObject;
                                        GameObject b = Instantiate(o, SpawnPosition, Quaternion.identity);
                                        b.GetComponent<SpriteRenderer>().color = new Color(178 / 255f, 178 / 255f, 178 / 255f);
                                        b.tag = "player";
                                        b.GetComponent<PlayerController>().IsClone = 1;

                                        PlayerController playerController = b.GetComponent<PlayerController>();
                                        playerController.enabled = true;

                                        map_Editor.Upbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Upbtn); });
                                        map_Editor.Downbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Downbtn); });
                                        map_Editor.Leftbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Leftbtn); });
                                        map_Editor.Rightbtn.onClick.AddListener(delegate () { playerController.Move(map_Editor.Rightbtn); });
                                    }
                                }
                            }
                        }
                    }
                }
            }



            yield return null;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //로봇 생성 : spawner에 닿으면 spawn 위치에 있는 robot이 움직이고 player는 움직일 수 없음. (조종 컨셉)
        if (this.gameObject.CompareTag("robotspawner") && collision.CompareTag("player"))
        {
            Robot = GameObject.FindWithTag("robotplayer");
            Player = collision.gameObject;

            if (this.gameObject.transform.position == Player.transform.position)
            {
                statusManager.ReadyToRobotOperation = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //클론 생성 : spawner에 닿으면 spawn 스위치에서 클론이 생성됨 
        if (this.gameObject.CompareTag("clonespawner") && collision.CompareTag("player")
            && collision.GetComponent<PlayerController>().IsClone == 0 /*&& statusManager.CloneNum == 0*/)
        {
            statusManager.ReadyToCloneSpawn = true;
        }

        if(this.gameObject.CompareTag("clonespawn"))
        {
            SpawnBlocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("clonespawn"))
        {
            SpawnBlocked = false;
        }

        if (this.gameObject.CompareTag("clonespawner") && collision.CompareTag("player")
           && collision.GetComponent<PlayerController>().IsClone == 0 /*&& statusManager.CloneNum == 0*/)
        {
            statusManager.ReadyToCloneSpawn = false;
        }


        if (this.gameObject.CompareTag("robotspawner") && collision.CompareTag("player"))
        {
            statusManager.ReadyToRobotOperation = false;
        }
    }
}
