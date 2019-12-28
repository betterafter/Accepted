using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionManager : MonoBehaviour
{
    private GameObject playerobj;
    private Sprite bar;

    public GameObject[] ColObj;
    public bool[] IsPushed, horizontalActive, VerticalActive, CrossActive;
    public bool isStepOn, isConnected;

    private float px, py;
    private bool IsLinearUp, IsLinearDown, IsLinearLeft, IsLinearRight;



    public virtual void Start()
    {
        ColObj = new GameObject[4];
        IsPushed = new bool[4]; horizontalActive = new bool[4]; VerticalActive = new bool[4]; CrossActive = new bool[4];
        isStepOn = false; isConnected = false;

        for (int i = 0; i < 4; i++)
        {
            IsPushed[i] = true;
            horizontalActive[i] = false; VerticalActive[i] = false; CrossActive[i] = false;
        }

        if (this.gameObject.tag.Contains("bar"))
        {
            bar = gameObject.GetComponent<SpriteRenderer>().sprite;
            //bar = Resources.Load<Sprite>("image/" + gameObject.tag);
        }
    }

    private void BarActive(int i, bool active)
    {
        if (ColObj[i] != null && 
            ((ColObj[i].CompareTag("brick") && ColObj[i].GetComponent<CollisionManager>().isStepOn == true)
            || ((ColObj[i].CompareTag(this.gameObject.tag) || ColObj[i].CompareTag("crossbar")) && ColObj[i].GetComponent<CollisionManager>().isConnected)))
        {
            if (!active)
            {
                if (this.gameObject.CompareTag("hobar")) 
                { 
                    horizontalActive[i] = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<sceneManager>().HorizontalBar;
                }
                else if (this.gameObject.CompareTag("verbar")) 
                {
                    VerticalActive[i] = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<sceneManager>().VerticalBar;
                }
                else if (this.gameObject.CompareTag("crossbar")) 
                { 
                    CrossActive[i] = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<sceneManager>().CrossBar;
                }
                isConnected = true;
            }
        }

        else if(this.gameObject.CompareTag("crossbar"))
        {
            if(i == 0 || i == 1)
            {
                if(ColObj[i] != null && (ColObj[i].CompareTag(this.gameObject.tag) || ColObj[i].CompareTag("hobar")) &&
                     ColObj[i].GetComponent<CollisionManager>().isConnected)
                {
                    if (!active)
                    {
                        CrossActive[i] = true;
                        gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<sceneManager>().CrossBar;
                        isConnected = true;
                    }
                }
            }
            else if(i == 2 || i == 3)
            {
                if (ColObj[i] != null && (ColObj[i].CompareTag(this.gameObject.tag) || ColObj[i].CompareTag("verbar")) &&
                    ColObj[i].GetComponent<CollisionManager>().isConnected)
                {
                    if (!active)
                    {
                        CrossActive[i] = true;
                        gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<sceneManager>().CrossBar;
                        isConnected = true;
                    }
                }
            }
        }



        else
        {

            if (this.gameObject.CompareTag("hobar")) horizontalActive[i] = false;
            else if (this.gameObject.CompareTag("verbar")) VerticalActive[i] = false;
            else if (this.gameObject.CompareTag("crossbar")) CrossActive[i] = false;
        }

    }

    private void FixedUpdate()
    {
        IsCollide(0);
        IsCollide(1);
        IsCollide(2);
        IsCollide(3);
    }

    public virtual void Update()
    {
        if (this.gameObject.CompareTag("hobar"))
        {
            BarActive(0, horizontalActive[0]);
            BarActive(1, horizontalActive[1]);
            if (!horizontalActive[0] && !horizontalActive[1]) { gameObject.GetComponent<SpriteRenderer>().sprite = bar; isConnected = false; }
        }
        else if (this.gameObject.CompareTag("verbar"))
        {
            BarActive(2, VerticalActive[2]);
            BarActive(3, VerticalActive[3]);
            if (!VerticalActive[2] && !VerticalActive[3]) { gameObject.GetComponent<SpriteRenderer>().sprite = bar; isConnected = false; }
        }
        else if (this.gameObject.CompareTag("crossbar"))
        {
            BarActive(0, CrossActive[0]);
            BarActive(1, CrossActive[1]);
            BarActive(2, CrossActive[2]);
            BarActive(3, CrossActive[3]);
            if (!CrossActive[0] && !CrossActive[1] && !CrossActive[2] && !CrossActive[3]) { gameObject.GetComponent<SpriteRenderer>().sprite = bar; isConnected = false; }
        }
    }


    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        // 이 충돌체가 accepted, step과 충돌했을 경우는 충돌중인 물체 판정대상에서 제외해준다 
        if (!collision.CompareTag("accepted") && !collision.tag.Contains("step") 
            && !collision.tag.Contains("spawn") && !collision.tag.Contains("detective"))
        {

            // 이 구간을 accepted, step이 아닐 경우만 연산을 해줘야 버벅거림이 없음.
            // 만약 밖으로 빼주게 된다면 충돌 때문에 충돌이 발생하는 즉시 멈춰서 반절 정도만 움직이게 됨..
            int cx = Mathf.RoundToInt(collision.transform.position.x);
            int cy = Mathf.RoundToInt(collision.transform.position.y);

            //collision.transform.position = new Vector2(cx, cy);

            int tx = Mathf.RoundToInt(gameObject.transform.position.x);
            int ty = Mathf.RoundToInt(gameObject.transform.position.y);

            //gameObject.transform.position = new Vector2(tx, ty);
            ////////////////////////////////////////////////////////////////////////

            // obj : up
            if (tx - cx == 0 && cy > ty)
            {
                ColObj[0] = collision.gameObject;
            }
            // obj : right
            else if (ty - cy == 0 && cx > tx)
            {
                ColObj[3] = collision.gameObject;
            }
            // obj : down
            else if (tx - cx == 0 && cy < ty)
            {
                ColObj[1] = collision.gameObject;
            }
            // obj : left
            else if (ty - cy == 0 && cx < tx)
            {
                ColObj[2] = collision.gameObject;
            }
            
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("accepted") && !collision.tag.Contains("step")
            && !collision.tag.Contains("spawn") && !collision.tag.Contains("detective"))
        {
            //Debug.Log(ColObj[0] + ", " + ColObj[1] + ", " + ColObj[2] + ", " + ColObj[3]);
            if (collision.gameObject == ColObj[0])
            {
                ColObj[0] = null;
            }
            else if (collision.gameObject == ColObj[1])
            {
                ColObj[1] = null;
            }
            else if (collision.gameObject == ColObj[2])
            {
                ColObj[2] = null;
            }
            else if (collision.gameObject == ColObj[3])
            {
                ColObj[3] = null;
            }
        }
    }

    private void IsCollide(int i)
    {
        if (ColObj[i] != null)
        {
            if (!ColObj[i].tag.Contains("player")) IsPushed[i] = false;
            else if (ColObj[i].tag.Contains("player"))
            {
                PlayerController playerController = ColObj[i].GetComponent<PlayerController>();
                if (playerController.enabled && playerController.IsPlayerMoved[i] == true) IsPushed[i] = true;
                else if (!playerController.enabled || playerController.IsPlayerMoved[i] == false) IsPushed[i] = false;
            }
        }

        else if (ColObj[i] == null) IsPushed[i] = true;
    }
}
