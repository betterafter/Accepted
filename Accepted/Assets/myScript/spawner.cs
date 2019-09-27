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

    void Start()
    {
        SpawnBlocked = false;
        statusManager = Camera.main.GetComponent<StatusManager>();
        sceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();
        map_Editor = Camera.main.GetComponent<Map_Editor>();

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
                if (SpawnBlocked == false)
                {
                    GameObject o = Resources.Load("Prefabs/obj/player") as GameObject;
                    GameObject b = Instantiate(o, SpawnPosition, Quaternion.identity);
                    b.GetComponent<SpriteRenderer>().color = new Color(178 / 255f, 178 / 255f, 178 / 255f);
                    b.tag = "player";
                    b.GetComponent<PlayerController>().IsClone = 1;
                    b.GetComponent<PlayerController>().enabled = false;
                }
            }

            yield return new WaitForFixedUpdate();
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
                //Robot.GetComponent<PlayerController>().enabled = true;
                //collision.GetComponent<PlayerController>().enabled = false;
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

        //if (this.gameObject.CompareTag("clonespawn") && collision.CompareTag("player")
        //    && collision.gameObject.GetComponent<PlayerController>().IsClone == 1) 
        //{
        //    if(collision.gameObject.GetComponent<ObjectStatus>().LeaveSpawner == true)
        //    {
        //        collision.gameObject.SetActive(false);
        //    }
            
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("clonespawn"))
        {
            SpawnBlocked = false;
        }

        if (this.gameObject.CompareTag("robotspawner") && collision.CompareTag("player"))
        {
            statusManager.ReadyToRobotOperation = false;
        }

        //if(this.gameObject.CompareTag("clonespawn") && collision.CompareTag("player"))
        //{
        //    collision.gameObject.GetComponent<ObjectStatus>().LeaveSpawner = true;
        //}
    }
}
