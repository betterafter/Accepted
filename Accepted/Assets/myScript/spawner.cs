using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    private GameObject Spawn;
    private GameObject Robot;
    private GameObject Player;

    private Vector3 SpawnPosition;
    private StatusManager statusManager;
    

    void Start()
    {
        Spawn = GameObject.FindWithTag("clonespawn");
        Robot = GameObject.FindWithTag("robot");
        Player = GameObject.FindWithTag("player");


        SpawnPosition = Spawn.transform.position;
        statusManager = Camera.main.GetComponent<StatusManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //클론 생성 : spawner에 닿으면 spawn 스위치에서 클론이 생성됨 
        if (this.gameObject.CompareTag("clonespawner") && collision.CompareTag("player"))
        {
            GameObject o = Resources.Load("Prefabs/obj/player") as GameObject;
            GameObject b = Instantiate(o, SpawnPosition, Quaternion.identity);
            b.tag = "player";
            b.GetComponent<PlayerController>().IsClone = 1;
        }

        //클론이 spawn에 닿으면 클론이 파괴됨. 이 경우 되돌릴 수 없음 
        else if(this.gameObject.CompareTag("clonespawn") && collision.GetComponent<PlayerController>().IsClone == 1
            && collision.GetComponent<PlayerController>().IsCloneFirstMoved == 1)
        {
            Destroy(collision);
        }

        //로봇 생성 : spawner에 닿으면 spawn 위치에 있는 robot이 움직이고 player는 움직일 수 없음. (조종 컨셉)
        else if(this.gameObject.CompareTag("robotspawner") && collision.CompareTag("player"))
        {
            if(Application.internetReachability != NetworkReachability.NotReachable)
            {
                Robot.GetComponent<PlayerController>().enabled = true;
                Player.GetComponent<PlayerController>().enabled = false;
            }
           
        }

        else if (this.gameObject.CompareTag("robotspawn") && collision.CompareTag("robot")
            && collision.GetComponent<PlayerController>().IsRobotFirstMoved == 1)
        {
            collision.GetComponent<PlayerController>().IsRobotFirstMoved = 0;
            Robot.GetComponent<PlayerController>().enabled = false;
            Player.GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //클론이 맨 처음 생성될 때 바로 파괴되면 안되므로 최소 한번은 움직일 수 있도록 함
        if(this.gameObject.CompareTag("clonespawn") && collision.GetComponent<PlayerController>().IsCloneFirstMoved == 0)
        {
            collision.GetComponent<PlayerController>().IsCloneFirstMoved = 1;
        }

        else if(this.gameObject.CompareTag("robotspawn") && collision.CompareTag("robot"))
        {
            collision.GetComponent<PlayerController>().IsRobotFirstMoved = 1;
        } 

    }
}
