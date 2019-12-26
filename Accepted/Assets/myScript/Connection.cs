using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{

    public CollisionManager collisionManager;
    public bool isConnected;

    private void Start()
    {
        isConnected = false;
        collisionManager = this.gameObject.GetComponent<CollisionManager>();
    }

    private void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if (collisionManager.ColObj[i].tag.Contains("bar") && collisionManager.ColObj[i].GetComponent<CollisionManager>().isConnected)
            {
                isConnected = true; break;
            }
            isConnected = false;
        }
    }



}
