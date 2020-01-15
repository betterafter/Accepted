using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{

    public CollisionManager collisionManager;
    public bool isConnected;
    public GameObject[,] smog = new GameObject[30, 20];
    public GameObject Smog;

    public Sprite deadResearcher;
    Animator anime;

    float MoveTime;

    int[ , ] MapDirection = { { 1, 0 }, { -1, 0 }, { 0, 1 } , { 0, -1 } };

    sceneManager SceneManager;

    private void Start()
    {
        isConnected = false; MoveTime = 0.0f;
        collisionManager = this.gameObject.GetComponent<CollisionManager>();
        SceneManager = GameObject.Find("GameManager").GetComponent<sceneManager>();

        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                smog[i, j] = null;
            }
        }

   
    }



    private void Update()
    {

        for (int i = 0; i < 4; i++)
        {
            if (collisionManager.ColObj[i] != null && collisionManager.ColObj[i].tag.Contains("bar"))
            {
                if(collisionManager.ColObj[i].GetComponent<CollisionManager>().isConnected) isConnected = true; break;
            }
            isConnected = false;
        }


        if (isConnected && MoveTime > 2.0f)
        {
            Queue<KeyValuePair<int, int> > q = new Queue<KeyValuePair<int, int> >();
            for(int i = 0; i <= 20; i++)
            {
                for(int j = 0; j <= 15; j++)
                {
                    if(smog[i, j] != null || (SceneManager.minimap[i, j] != null &&
                         SceneManager.minimap[i, j].CompareTag("CultureTube") && smog[i , j] == null))
                    {
                        q.Enqueue(new KeyValuePair<int, int>(i, j));
                    }
                }
            }

            while(q.Count != 0)
            {
                int i = q.Peek().Key, j = q.Peek().Value; q.Dequeue();
                for (int k = 0; k < 4; k++)
                {
                    int DirX = j + MapDirection[k, 0];
                    int DirY = i + MapDirection[k, 1];

                    if (DirX >= 0 && DirX < 15 && DirY >= 0 && DirY < 20 && smog[DirY, DirX] == null 
                        && (SceneManager.minimap[DirY, DirX] == null || (SceneManager.minimap[DirY, DirX] != null && !SceneManager.minimap[DirY, DirX].CompareTag("brick") &&
                        !SceneManager.minimap[DirY, DirX].CompareTag("block") && !SceneManager.minimap[DirY, DirX].tag.Contains("obj"))))
                    {

                        GameObject smogObject = Instantiate(Smog, new Vector3(DirX - 7, DirY - 5), Quaternion.identity);
                        smog[DirY, DirX] = smogObject;

                    }
                    else if (DirX >= 0 && DirX < 15 && DirY >= 0 && DirY < 20 && smog[DirY, DirX] != null
                        && (SceneManager.minimap[DirY, DirX] == null || (SceneManager.minimap[DirY, DirX] != null && !SceneManager.minimap[DirY, DirX].CompareTag("brick") &&
                        !SceneManager.minimap[DirY, DirX].CompareTag("block") && !SceneManager.minimap[DirY, DirX].tag.Contains("obj"))))
                    { 
                        smog[DirY, DirX].GetComponent<SpriteRenderer>().enabled = true;

                    }
                }
            }

            MoveTime = 0.0f;
        }
        MoveTime += 0.1f;

        // 연구원 사망 모션 
        for(int i = 0; i <= 20; i++)
        {
            for(int j = 0; j <= 15; j++)
            {
                //if(SceneManager.minimap[i, j] != null) Debug.Log(SceneManager.minimap[i, j].tag);
                if(smog[i, j] != null && 
                    (SceneManager.minimap[i, j] != null && (SceneManager.minimap[i, j].CompareTag("brick") || 
                    SceneManager.minimap[i, j].CompareTag("block") || SceneManager.minimap[i, j].tag.Contains("obj"))))
                {
                    smog[i, j].GetComponent<SpriteRenderer>().enabled = false;
                }
                if(smog[i, j] != null && SceneManager.minimap[i, j] != null && SceneManager.minimap[i, j].CompareTag("researcher"))
                {
                    anime = SceneManager.minimap[i, j].GetComponent<Animator>();
                    anime.SetBool("isDead", true);
                    SceneManager.minimap[i, j].transform.GetChild(0).gameObject.SetActive(false);
                    SceneManager.minimap[i, j].GetComponent<EnemyCollision>().enabled = false;
                }
            }
        }
    }
}
