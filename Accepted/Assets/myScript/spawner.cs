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

    private Vector3 SpawnPosition;

    private StatusManager statusManager;
    private sceneManager sceneManager;
    private Map_Editor map_Editor;

    private Button Upbtn, Downbtn, Leftbtn, Rightbtn;



    void Start()
    {
        SpawnBlocked = false;
        statusManager = Camera.main.GetComponent<StatusManager>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        map_Editor = Camera.main.GetComponent<Map_Editor>();

        Upbtn = map_Editor.Upbtn;
        Downbtn = map_Editor.Downbtn;
        Leftbtn = map_Editor.Leftbtn;
        Rightbtn = map_Editor.Rightbtn;



        StartCoroutine("CloneSpawn");
    }

    private void FixedUpdate()
    {
        statusManager.ReadyToCloneSpawn = false;
    }

    private IEnumerator CloneSpawn()
    {
        while (true)
        {
            if(gameObject.CompareTag("clonespawn") && statusManager.ReadyToCloneSpawn)
            {
                SpawnPosition = gameObject.transform.position;

                GameObject o = Resources.Load("Prefabs/obj/player") as GameObject;
                GameObject b = Instantiate(o, SpawnPosition, Quaternion.identity);
                b.GetComponent<SpriteRenderer>().color = new Color(178 / 255f, 178 / 255f, 178 / 255f);
                b.tag = "player";
                b.GetComponent<PlayerController>().IsClone = 1;
                b.GetComponent<PlayerController>().enabled = false;

                //Upbtn = GameObject.Find("up").GetComponent<Button>();
                //Downbtn = GameObject.Find("down").GetComponent<Button>();
                //Leftbtn = GameObject.Find("left").GetComponent<Button>();
                //Rightbtn = GameObject.Find("right").GetComponent<Button>();

                Upbtn.onClick.AddListener(delegate () { b.GetComponent<PlayerController>().Move(Upbtn); });
                Downbtn.onClick.AddListener(delegate () { b.GetComponent<PlayerController>().Move(Downbtn); });
                Leftbtn.onClick.AddListener(delegate () { b.GetComponent<PlayerController>().Move(Leftbtn); });
                Rightbtn.onClick.AddListener(delegate () { b.GetComponent<PlayerController>().Move(Rightbtn); });
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //클론 생성 : spawner에 닿으면 spawn 스위치에서 클론이 생성됨 
        if (this.gameObject.CompareTag("clonespawner") && collision.CompareTag("player") && SpawnBlocked == false
            && collision.GetComponent<PlayerController>().IsClone == 0 /*&& statusManager.CloneNum == 0*/)
        {
            statusManager.ReadyToCloneSpawn = true;

            //Camera.main.GetComponent<StatusManager>().CloneNum++;
        }

        //로봇 생성 : spawner에 닿으면 spawn 위치에 있는 robot이 움직이고 player는 움직일 수 없음. (조종 컨셉)
        if(this.gameObject.CompareTag("robotspawner") && collision.CompareTag("player"))
        {
            if(Application.internetReachability != NetworkReachability.NotReachable)
            {
                Robot = GameObject.FindWithTag("robot");
                Player = GameObject.FindWithTag("player");

                Robot.GetComponent<PlayerController>().enabled = true;
                Player.GetComponent<PlayerController>().enabled = false;
            }
           
        }

        else if (this.gameObject.CompareTag("robotspawn") && collision.CompareTag("robot")
            && collision.GetComponent<PlayerController>().IsRobotFirstMoved == 1)
        {
            Robot = GameObject.FindWithTag("robot");
            Player = GameObject.FindWithTag("player");

            collision.GetComponent<PlayerController>().IsRobotFirstMoved = 0;
            Robot.GetComponent<PlayerController>().enabled = false;
            Player.GetComponent<PlayerController>().enabled = true;
        }

        if(this.gameObject.CompareTag("clonespawn") && !collision.gameObject.CompareTag("player"))
        {
            SpawnBlocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(this.gameObject.CompareTag("robotspawn") && collision.CompareTag("robot"))
        {
            collision.GetComponent<PlayerController>().IsRobotFirstMoved = 1;
        }

        if (this.gameObject.CompareTag("clonespawn") && !collision.gameObject.CompareTag("player"))
        {
            SpawnBlocked = false;
        }
    }
}
