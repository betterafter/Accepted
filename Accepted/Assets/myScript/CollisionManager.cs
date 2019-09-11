using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionManager : MonoBehaviour
{
    private GameObject playerobj;
    public GameObject[] ColObj;
    private float px, py;
    private bool IsLinearUp, IsLinearDown, IsLinearLeft, IsLinearRight;


    private void Awake()
    {
       ColObj = new GameObject[4];
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이 충돌체가 player, accepted, step과 충돌했을 경우는 충돌중인 물체 판정대상에서 제외해준다 
        if (!collision.CompareTag("player") && !collision.CompareTag("accepted") && !collision.tag.Contains("step") 
            && !collision.CompareTag("robot") && !collision.tag.Contains("spawn"))
        {

            // 이 구간을 player, accepted, step이 아닐 경우만 연산을 해줘야 버벅거림이 없음.
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
                //Debug.Log(ColObj[0]);
            }
            // obj : right
            else if (ty - cy == 0 && cx > tx)
            {
                ColObj[3] = collision.gameObject;
                //Debug.Log(ColObj[3]);
            }
            // obj : down
            else if (tx - cx == 0 && cy < ty)
            {
                ColObj[1] = collision.gameObject;
                //Debug.Log(ColObj[1]);
            }
            // obj : left
            else if (ty - cy == 0 && cx < tx)
            {
                ColObj[2] = collision.gameObject;
                //Debug.Log(ColObj[2]);
            }
            
        }
    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("player") && !collision.CompareTag("accepted") && !collision.tag.Contains("step")
            && !collision.CompareTag("robot"))
        {
            //Debug.Log(ColObj[0] + ", " + ColObj[1] + ", " + ColObj[2] + ", " + ColObj[3]);
            if (collision.gameObject == ColObj[0])
            {
                //Debug.Log("!");
                ColObj[0] = null;
            }
            else if (collision.gameObject == ColObj[1])
            {
                //Debug.Log("!!");
                ColObj[1] = null;
            }
            else if (collision.gameObject == ColObj[2])
            {
                //Debug.Log("!!!");
                ColObj[2] = null;
            }
            else if (collision.gameObject == ColObj[3])
            {
                //Debug.Log("!!!!");
                ColObj[3] = null;
            }
            else
            {
                //Debug.Log(collision);
            }
        }
    }
}
