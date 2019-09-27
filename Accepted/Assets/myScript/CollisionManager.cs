using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionManager : MonoBehaviour
{
    private GameObject playerobj;

    public GameObject[] ColObj;
    public bool[] IsPushed;

    private float px, py;
    private bool IsLinearUp, IsLinearDown, IsLinearLeft, IsLinearRight;


    private void Start()
    {
        ColObj = new GameObject[4];
        IsPushed = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            IsPushed[i] = true;
            ColObj[i] = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이 충돌체가 accepted, step과 충돌했을 경우는 충돌중인 물체 판정대상에서 제외해준다 
        if (!collision.CompareTag("accepted") && !collision.tag.Contains("step") 
            && !collision.tag.Contains("spawn"))
        {

            // 이 구간을 accepted, step이 아닐 경우만 연산을 해줘야 버벅거림이 없음.
            // 만약 밖으로 빼주게 된다면 충돌 때문에 충돌이 발생하는 즉시 멈춰서 반절 정도만 움직이게 됨..
            int cx = Mathf.RoundToInt(collision.transform.position.x);
            int cy = Mathf.RoundToInt(collision.transform.position.y);

            collision.transform.position = new Vector2(cx, cy);

            int tx = Mathf.RoundToInt(gameObject.transform.position.x);
            int ty = Mathf.RoundToInt(gameObject.transform.position.y);

            gameObject.transform.position = new Vector2(tx, ty);
            ////////////////////////////////////////////////////////////////////////

            // obj : up
            if (tx - cx == 0 && cy > ty)
            {
                ColObj[0] = collision.gameObject;
                if (!ColObj[0].tag.Contains("player")) IsPushed[0] = false;
                else if (ColObj[0].tag.Contains("player"))
                {
                    PlayerController playerController = ColObj[0].GetComponent<PlayerController>();
                    if (playerController.IsPlayerMoved[0] == true) IsPushed[0] = true;
                    else if (!playerController.enabled || playerController.IsPlayerMoved[0] == false) IsPushed[0] = false;
                }
                //Debug.Log(ColObj[0]);
            }
            // obj : right
            else if (ty - cy == 0 && cx > tx)
            {
                ColObj[3] = collision.gameObject;
                if (!ColObj[3].tag.Contains("player")) IsPushed[3] = false;
                else if (ColObj[3].tag.Contains("player"))
                {
                    PlayerController playerController = ColObj[3].GetComponent<PlayerController>();
                    if (playerController.IsPlayerMoved[3] == true) IsPushed[3] = true;
                    else if(!playerController.enabled || playerController.IsPlayerMoved[3] == false) IsPushed[3] = false;
                }
                //Debug.Log(ColObj[3]);
            }
            // obj : down
            else if (tx - cx == 0 && cy < ty)
            {
                ColObj[1] = collision.gameObject;
                if (!ColObj[1].tag.Contains("player")) IsPushed[1] = false;
                else if (ColObj[1].tag.Contains("player"))
                {
                    PlayerController playerController = ColObj[1].GetComponent<PlayerController>();
                    if (playerController.IsPlayerMoved[1] == true) IsPushed[1] = true;
                    else if (!playerController.enabled || playerController.IsPlayerMoved[1] == false) IsPushed[1] = false;
                }
                //Debug.Log(ColObj[1]);
            }
            // obj : left
            else if (ty - cy == 0 && cx < tx)
            {
                ColObj[2] = collision.gameObject;
                if (!ColObj[2].tag.Contains("player")) IsPushed[2] = false;
                else if (ColObj[2].tag.Contains("player"))
                {
                    PlayerController playerController = ColObj[2].GetComponent<PlayerController>();
                    if (playerController.IsPlayerMoved[2] == true) IsPushed[2] = true;
                    else if (!playerController.enabled || playerController.IsPlayerMoved[2] == false) IsPushed[2] = false;
                }
                //Debug.Log(ColObj[2]);
            }
            
        }
    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("accepted") && !collision.tag.Contains("step")
            && !collision.tag.Contains("spawn"))
        {
            //Debug.Log(ColObj[0] + ", " + ColObj[1] + ", " + ColObj[2] + ", " + ColObj[3]);
            if (collision.gameObject == ColObj[0])
            {
                //Debug.Log("!");
                ColObj[0] = null;
                IsPushed[0] = true;
            }
            else if (collision.gameObject == ColObj[1])
            {
                //Debug.Log("!!");
                ColObj[1] = null;
                IsPushed[1] = true;
            }
            else if (collision.gameObject == ColObj[2])
            {
                //Debug.Log("!!!");
                ColObj[2] = null;
                IsPushed[2] = true;
            }
            else if (collision.gameObject == ColObj[3])
            {
                //Debug.Log("!!!!");
                ColObj[3] = null;
                IsPushed[3] = true;
            }
            else
            {
                //Debug.Log(collision);
            }
        }
    }
}
