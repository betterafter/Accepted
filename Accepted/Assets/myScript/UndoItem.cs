using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoItem : MonoBehaviour
{
    private GameObject player, obj;
    private Vector3 playerpos, objpos;

    //setter
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    public void setObj(GameObject obj)
    {
        this.obj = obj;
    }

    public void setPlayerpos(Vector3 playerpos)
    {
        this.playerpos = playerpos;
    }

    public void setObjpos(Vector3 objpos)
    {
        this.objpos = objpos;
    }

    //getter
    public GameObject getPlayer()
    {
        return player;
    }

    public GameObject getObj()
    {
        return obj;
    }

    public Vector3 getPlayerpos()
    {
        return playerpos;
    }

    public Vector3 getObjpos()
    {
        return objpos;
    }
}
