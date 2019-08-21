using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Vector3 CurrPos, NextPos, CurrOtherPos, OtherNextPos, scale;

    public float time = 0.02f, lerpTime = 0.5f, dist1, dist2;
    public int dir, last;

    //private Stage s;
    private CollisionManager cm;
    private sceneManager sm;
    private Map_Editor ME;

    private GameObject[] colObj = new GameObject[4];
    private GameObject gm;
    private Animator anime;
    private Button btnup, btndown, btnleft, btnright;



    private void Start()
    {
        ME = Camera.main.GetComponent<Map_Editor>();

        btnup = ME.gamebutton.transform.Find("up").GetComponent<Button>();
        btndown = ME.gamebutton.transform.Find("down").GetComponent<Button>();
        btnleft = ME.gamebutton.transform.Find("left").GetComponent<Button>();
        btnright = ME.gamebutton.transform.Find("right").GetComponent<Button>();


        //s = Camera.main.GetComponent<Stage>();
        gm = GameObject.Find("GameManager");
        sm = gm.GetComponent<sceneManager>();

        anime = GetComponent<Animator>();
        last = 0;
    }




    private void PlayerMove(int i, float d1, float d2)
    {
        if (colObj[i - 1] != null && !colObj[i - 1].CompareTag("accepted") && !colObj[i - 1].tag.Contains("step"))
        {

            CurrOtherPos = colObj[i - 1].transform.position;
            cm = colObj[i - 1].GetComponent<CollisionManager>();

            //Debug.Log(cm.ColObj[i - 1]);

            if (i == 1)
            {
                // up
                if (cm.ColObj[i - 1] == null && (int)(CurrPos.x - CurrOtherPos.x) == 0 && CurrOtherPos.y > CurrPos.y)
                {
                    MoveAndPush(i, d1, d2);
                }
            }
            else if (i == 2)
            {
                // down
                if (cm.ColObj[i - 1] == null && (int)(CurrPos.x - CurrOtherPos.x) == 0 && CurrOtherPos.y < CurrPos.y)
                {
                    MoveAndPush(i, d1, d2);
                }
            }
            else if (i == 3)
            {
                // left
                if (cm.ColObj[i - 1] == null && (int)(CurrPos.y - CurrOtherPos.y) == 0 && CurrOtherPos.x < CurrPos.x)
                {
                    MoveAndPush(i, d1, d2);
                }
            }
            else if (i == 4)
            {
                //right
                if (cm.ColObj[i - 1] == null && (int)(CurrPos.y - CurrOtherPos.y) == 0 && CurrOtherPos.x > CurrPos.x)
                {
                    MoveAndPush(i, d1, d2);
                }
            }
            
        }


        else if (colObj[i - 1] == null)
        {
            NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
            gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
        }

        Invoke("StopMove", time);
    }




    private void MoveAndPush(int i, float d1, float d2)
    {
        OtherNextPos = new Vector3(CurrOtherPos.x + d1, CurrOtherPos.y + d2, CurrOtherPos.z);
        colObj[i - 1].transform.position = Vector3.Lerp(CurrOtherPos, OtherNextPos, lerpTime);

        NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
        gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
    }




    private void FixedUpdate()
    {
        CurrPos = gameObject.transform.position;
        scale = transform.localScale;


        if (dir == 1)
        {
            dist1 = 0; dist2 = 1;
            PlayerMove(dir, dist1, dist2);
        }
        else if (dir == 2)
        {
            dist1 = 0; dist2 = -1;
            PlayerMove(dir, dist1, dist2);
        }
        else if (dir == 3)
        {
            dist1 = -1; dist2 = 0;

            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;


            PlayerMove(dir, dist1, dist2);
        }
        else if (dir == 4)
        {
            dist1 = 1; dist2 = 0;

            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            PlayerMove(dir, dist1, dist2);
        }

        btnup.enabled = true;
        btndown.enabled = true;
        btnleft.enabled = true;
        btnright.enabled = true;
    }


    //버튼 연속 입력을 막기 위한 함수 
    private void ButtonCoolDown(Button btn)
    {
        if (btn == btnup)
        {
            btndown.enabled = false;
            btnleft.enabled = false;
            btnright.enabled = false;
        }

        else if (btn == btndown)
        {
            btnup.enabled = false;
            btnleft.enabled = false;
            btnright.enabled = false;
        }

        else if (btn == btnleft)
        {
            btndown.enabled = false;
            btnup.enabled = false;
            btnright.enabled = false;
        }

        else if (btn == btnright)
        {
            btndown.enabled = false;
            btnleft.enabled = false;
            btnup.enabled = false;
        }
    }



    public void Move(Button btn)
    {
        ButtonCoolDown(btn);
        //new를 통한 생성과 같은 기능을 수행함.
        UndoItem undo = gameObject.AddComponent<UndoItem>();

        //undo를 수행하기 위한 이전 단계를 스택에 저장하기 위해 값들을 세팅해주는 과정.
        undo.setPlayer(gameObject);
        undo.setPlayerpos(gameObject.transform.position);

        if (btn.tag == btnup.tag)
        {
            if (colObj[0] != null)
            {
                undo.setObj(colObj[0]);
                undo.setObjpos(colObj[0].transform.position);
            }

            sm.stack.Push(undo);

            anime.SetBool("IsBackWalk", true);
            anime.SetBool("IsIdle", false);
            anime.SetBool("IsFrontIdle", false);
            anime.SetBool("IsBackIdle", false);

            if (colObj[0] == null && transform.position.y < 7) dir = 1;
            else if (colObj[0] != null && colObj[0].transform.position.y < 7) dir = 1;
            else Invoke("StopMove", time); 
        }
        else if (btn.tag == btndown.tag)
        {
            if (colObj[1] != null)
            {
                undo.setObj(colObj[1]);
                undo.setObjpos(colObj[1].transform.position);
            }

            sm.stack.Push(undo);

            anime.SetBool("IsFrontWalk", true);
            anime.SetBool("IsIdle", false);
            anime.SetBool("IsFrontIdle", false);
            anime.SetBool("IsBackIdle", false);

            if (colObj[1] == null && transform.position.y > -4) dir = 2;
            else if (colObj[1] != null && colObj[1].transform.position.y > -4) dir = 2;
            else Invoke("StopMove", time);
        }
        else if (btn.tag == btnleft.tag)
        {
            if (colObj[2] != null)
            {
                undo.setObj(colObj[2]);
                undo.setObjpos(colObj[2].transform.position);
            }

            sm.stack.Push(undo);

            anime.SetBool("IsWalk", true);
            anime.SetBool("IsIdle", false);
            anime.SetBool("IsFrontIdle", false);
            anime.SetBool("IsBackIdle", false);

            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;

            if (colObj[2] == null && transform.position.x > -4) dir = 3;
            else if (colObj[2] != null && colObj[2].transform.position.x > -4) dir = 3;
            else Invoke("StopMove", time);
        }
        else if (btn.tag == btnright.tag)
        {
            if (colObj[3] != null)
            {
                undo.setObj(colObj[3]);
                undo.setObjpos(colObj[3].transform.position);
            }

            sm.stack.Push(undo);

            anime.SetBool("IsWalk", true);
            anime.SetBool("IsIdle", false);
            anime.SetBool("IsFrontIdle", false);
            anime.SetBool("IsBackIdle", false);

            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;

            if (colObj[3] == null && transform.position.x < 4) dir = 4;
            else if (colObj[3] != null && colObj[3].transform.position.x < 4) dir = 4;
            else Invoke("StopMove", time); 
        }
    }




    private void StopMove()
    {

        if (anime.GetBool("IsWalk") == true)
        {
            anime.SetBool("IsWalk", false);
            anime.SetBool("IsIdle", true);
        }
        else if (anime.GetBool("IsFrontWalk") == true)
        {
            anime.SetBool("IsFrontWalk", false);
            anime.SetBool("IsFrontIdle", true);
        }
        else if (anime.GetBool("IsBackWalk") == true)
        {
            anime.SetBool("IsBackWalk", false);
            anime.SetBool("IsBackIdle", true);
        }

        dir = 0;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("accepted") && !other.tag.Contains("step"))
        {
            //player의 위치와 충돌체의 위치 계산
            int ox = Mathf.RoundToInt(other.transform.position.x);
            int oy = Mathf.RoundToInt(other.transform.position.y);

            other.transform.position = new Vector2(ox, oy);

            int tx = Mathf.RoundToInt(gameObject.transform.position.x);
            int ty = Mathf.RoundToInt(gameObject.transform.position.y);

            gameObject.transform.position = new Vector2(tx, ty);

            //Debug.Log(other.tag);
            if (ox - tx == 0 && oy > ty)
            {
                //Debug.Log("0");
                colObj[0] = other.gameObject;
            }
            else if (ox - tx == 0 && oy < ty)
            {
                //Debug.Log("1");
                colObj[1] = other.gameObject;
            }
            else if (oy - ty == 0 && ox < tx)
            {
                //Debug.Log("2");
                colObj[2] = other.gameObject;
            }
            else if (oy - ty == 0 && ox > tx)
            {
                //Debug.Log("3");
                colObj[3] = other.gameObject;
            }
        }


    }

     

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject == colObj[0])
        {
            colObj[0] = null;
        }
        else if (other.gameObject == colObj[1])
        {
            colObj[1] = null;
        }
        else if (other.gameObject == colObj[2])
        {
            colObj[2] = null;
        }
        else if (other.gameObject == colObj[3])
        {
            colObj[3] = null;
        }
    }
}
