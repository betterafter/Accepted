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
    public int IsClone;
    public int IsCloneFirstMoved;
    public int IsRobotFirstMoved;

    //private Stage s;
    private CollisionManager cm;
    private sceneManager sm;
    private Map_Editor ME;
    private StatusManager statusManager;

    private GameObject[] colObj = new GameObject[4];
    private GameObject gm;
    private Animator anime;
    private Button btnup, btndown, btnleft, btnright;

    


    private void Start()
    {
        IsCloneFirstMoved = 0; IsRobotFirstMoved = 0;

        statusManager = Camera.main.GetComponent<StatusManager>();
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

    private void Update()
    {
      
        if ((int)Camera.main.transform.eulerAngles.z != 0)
        {
            if ((int)(Camera.main.transform.eulerAngles.z % 270) == 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 270);
            }
            else if ((int)(Camera.main.transform.eulerAngles.z % 180) == 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if ((int)(Camera.main.transform.eulerAngles.z % 90) == 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    //오브젝트를 밀고가는 움직임 연산
    #region MoveCalc

    private void PlayerMove(int i, float d1, float d2)
    {
        if (colObj[i - 1] != null && !colObj[i - 1].CompareTag("accepted") && !colObj[i - 1].tag.Contains("step")
            && !colObj[i - 1].tag.Contains("spawn") && !colObj[i - 1].CompareTag("player"))
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


        else if (colObj[i - 1] == null || colObj[i - 1].CompareTag("player"))
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

    #endregion

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
            PlayerMove(dir, dist1, dist2);
        }
        else if (dir == 4)
        {
            dist1 = 1; dist2 = 0;
            PlayerMove(dir, dist1, dist2);
        }

        //버튼 연속 입력을 막기 위한 세팅 
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

    #region MoveAnimeSetting

    public void Move(Button btn)
    {
        anime.SetBool("IsIdle", false);
        ButtonCoolDown(btn);
        //new를 통한 생성과 같은 기능을 수행함.
        UndoItem undo = gameObject.AddComponent<UndoItem>();

        //undo를 수행하기 위한 이전 단계를 스택에 저장하기 위해 값들을 세팅해주는 과정.
        if (IsClone == 0)
        {
            undo.setPlayer(gameObject);
            undo.setPlayerpos(gameObject.transform.position);
        }
        if (btn.tag == btnup.tag)
        {
            if (IsClone == 0)
            {
                if (colObj[0] != null)
                {
                    undo.setObj(colObj[0]);
                    undo.setObjpos(colObj[0].transform.position);
                }

                sm.stack.Push(undo);
            }
            SetMoveAnime("up", statusManager.RotateAngle);
            dir = 1;

        }
        else if (btn.tag == btndown.tag)
        {
            if (IsClone == 0)
            {
                if (colObj[1] != null)
                {
                    undo.setObj(colObj[1]);
                    undo.setObjpos(colObj[1].transform.position);
                }

                sm.stack.Push(undo);
            }
            SetMoveAnime("down", statusManager.RotateAngle);
            dir = 2;
        }
        else if (btn.tag == btnleft.tag)
        {
            if (IsClone == 0)
            {
                if (colObj[2] != null)
                {
                    undo.setObj(colObj[2]);
                    undo.setObjpos(colObj[2].transform.position);
                }

                sm.stack.Push(undo);
            }
            SetMoveAnime("left", statusManager.RotateAngle);
            dir = 3;
        }
        else if (btn.tag == btnright.tag)
        {
            if (IsClone == 0)
            {
                if (colObj[3] != null)
                {
                    undo.setObj(colObj[3]);
                    undo.setObjpos(colObj[3].transform.position);
                }

                sm.stack.Push(undo);
            }
            SetMoveAnime("right", statusManager.RotateAngle);
            dir = 4;
        }
    }

    // 회전 블록 active 일 때 사용하는 함수 
    private void SetMoveAnime(string Movedir, int rotation)
    {
        if (Movedir == "up")
        {
            if (rotation != 0)
            {
                if (rotation % 270 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else if (rotation % 180 == 0) anime.SetBool("IsFrontWalk", true);
                else if (rotation % 90 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else
                {
                    anime.SetBool("IsBackWalk", true);
                }
            }
            else if (rotation == 0) anime.SetBool("IsBackWalk", true);

        }

        if (Movedir == "down")
        {
            if (rotation != 0)
            {
                if (rotation % 270 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else if (rotation % 180 == 0) anime.SetBool("IsBackWalk", true);
                else if (rotation % 90 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else
                {
                    anime.SetBool("IsFrontWalk", true);
                }
            }
            else if (rotation == 0) anime.SetBool("IsFrontWalk", true);

        }

        if (Movedir == "left")
        {
            if (rotation != 0)
            {
                if (rotation % 270 == 0) anime.SetBool("IsFrontWalk", true);
                else if (rotation % 180 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else if (rotation % 90 == 0) anime.SetBool("IsBackWalk", true);
                else
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
            else if (rotation == 0)
            {
                anime.SetBool("IsWalk", true);
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

        }

        if (Movedir == "right")
        {
            if (rotation != 0)
            {
                if (rotation % 270 == 0) anime.SetBool("IsBackWalk", true);
                else if (rotation % 180 == 0)
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else if (rotation % 90 == 0) anime.SetBool("IsFrontWalk", true);
                else
                {
                    anime.SetBool("IsWalk", true);
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
            else if (rotation == 0)
            {
                anime.SetBool("IsWalk", true);
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    //움직임이 끝날 때 정지 애니메이션
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

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("accepted") && !other.tag.Contains("step") && !other.tag.Contains("spawn"))
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("player") && collision.transform.position == this.gameObject.transform.position
            && IsClone == 0)
        {
            collision.gameObject.SetActive(false);
            Camera.main.GetComponent<StatusManager>().CloneNum = 0;
        }
    }
}
