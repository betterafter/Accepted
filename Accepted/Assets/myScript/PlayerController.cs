using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Vector3 CurrPos, NextPos, CurrOtherPos, OtherNextPos, scale, PrevPlayerPosition;

    public float time = 0.02f, lerpTime = 0.5f, dist1, dist2;
    public int last;
    public int IsClone;
    public int IsCloneFirstMoved;
    public int IsRobotFirstMoved;
    public int IsClickedButton_Move;


    //private Stage s;
    private CollisionManager collisionManager;
    private sceneManager sm;
    private Map_Editor ME;
    private StatusManager statusManager;

    private GameObject[] colObj = new GameObject[4];
    public bool[] IsPlayerMoved = new bool[4];
    private GameObject gm;
    private Animator anime;
    private Button btnup, btndown, btnleft, btnright;

    AudioSource audioSource;


    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();

        IsCloneFirstMoved = 0; IsRobotFirstMoved = 0;
        //colObj = new GameObject[4];
        //IsPlayerMoved = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            IsPlayerMoved[i] = true;
            colObj[i] = null;
        }

        statusManager = Camera.main.GetComponent<StatusManager>();
        ME = Camera.main.GetComponent<Map_Editor>();

        btnup = ME.gamebutton.transform.Find("up").GetComponent<Button>();
        btndown = ME.gamebutton.transform.Find("down").GetComponent<Button>();
        btnleft = ME.gamebutton.transform.Find("left").GetComponent<Button>();
        btnright = ME.gamebutton.transform.Find("right").GetComponent<Button>();

        //s = Camera.main.GetComponent<Stage>();
        gm = GameObject.Find("GameManager");
        sm = gm.GetComponent<sceneManager>();

        anime = this.gameObject.GetComponent<Animator>();
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

    private void Start()
    {
        PrevPlayerPosition = this.gameObject.transform.position;
    }
    //오브젝트를 밀고가는 움직임 연산
    #region MoveCalc

    private void PlayerMove(int i, float d1, float d2)
    {
        if (colObj[i - 1] != null && (colObj[i - 1].tag.Contains("brick") || colObj[i - 1].tag.Contains("obj")))
        {

            if (gameObject.GetComponent<PlayerController>().IsPlayerMoved[i - 1] == true)
            {
                CurrOtherPos = colObj[i - 1].transform.position;
                OtherNextPos = new Vector3(CurrOtherPos.x + d1, CurrOtherPos.y + d2, CurrOtherPos.z);
                colObj[i - 1].transform.position = Vector3.Lerp(CurrOtherPos, OtherNextPos, lerpTime);

                NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
                gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
            }
        }

        else if (colObj[i - 1] != null && colObj[i - 1].tag.Contains("player"))
        {
            if (IsPlayerMoved[i - 1] == true)
            {
                NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
                gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
            }
        }

        else if (colObj[i - 1] != null && colObj[i - 1].tag.Contains("block"))
        {
            if (gameObject.CompareTag("robotplayer"))
            {
                if (IsPlayerMoved[i - 1] == true)
                {
                    CurrOtherPos = colObj[i - 1].transform.position;
                    OtherNextPos = new Vector3(CurrOtherPos.x + d1, CurrOtherPos.y + d2, CurrOtherPos.z);
                    colObj[i - 1].transform.position = Vector3.Lerp(CurrOtherPos, OtherNextPos, lerpTime);

                    NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
                    gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
                }
            }
        }

        else if (colObj[i - 1] == null)
            {
                if (IsPlayerMoved[i - 1] == true)
                {
                    NextPos = new Vector3(CurrPos.x + d1, CurrPos.y + d2, CurrPos.z);
                    gameObject.transform.position = Vector3.Lerp(CurrPos, NextPos, lerpTime);
                }
            }
        

        Invoke("StopMove", time);
    }

    #endregion

    private void FixedUpdate()
    {
        IsCollide(0);
        IsCollide(1);
        IsCollide(2);
        IsCollide(3);

        CurrPos = gameObject.transform.position;
        scale = transform.localScale;

        if (sm.dir == 1)
        {
            dist1 = 0; dist2 = 1;
            PlayerMove(sm.dir, dist1, dist2);
        }
        else if (sm.dir == 2)
        {
            dist1 = 0; dist2 = -1;
            PlayerMove(sm.dir, dist1, dist2);
        }
        else if (sm.dir == 3)
        {
            dist1 = -1; dist2 = 0;
            PlayerMove(sm.dir, dist1, dist2);
        }
        else if (sm.dir == 4)
        {
            dist1 = 1; dist2 = 0;
            PlayerMove(sm.dir, dist1, dist2);
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
        if (this.enabled)
        {
            sm.IsLastClickedButton_Undo = false;
            sm.IsLastClickButton_Move = true;

            anime.SetBool("IsIdle", false);
            ButtonCoolDown(btn);

            if (btn.tag == btnup.tag)
            {
                this.audioSource.Play();
                SetMoveAnime("up", statusManager.RotateAngle);
                sm.dir = 1;
            }
            else if (btn.tag == btndown.tag)
            {
                this.audioSource.Play();
                SetMoveAnime("down", statusManager.RotateAngle);
                sm.dir = 2;
            }
            else if (btn.tag == btnleft.tag)
            {
                this.audioSource.Play();
                SetMoveAnime("left", statusManager.RotateAngle);
                sm.dir = 3;
            }
            else if (btn.tag == btnright.tag)
            {
                this.audioSource.Play();
                SetMoveAnime("right", statusManager.RotateAngle);
                sm.dir = 4;
            }
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

        if(this.gameObject.transform.position != PrevPlayerPosition)
        {
            sm.IsLastClickButton_Move = false;
            PrevPlayerPosition = this.gameObject.transform.position;
        }

        sm.dir = 0;
    }

    #endregion

    private void OnTriggerStay2D(Collider2D other)
    {
       if (!other.CompareTag("accepted") && !other.tag.Contains("step") && !other.tag.Contains("spawn"))
       {
            //player의 위치와 충돌체의 위치 계산
            int ox = Mathf.RoundToInt(other.transform.position.x);
            int oy = Mathf.RoundToInt(other.transform.position.y);

            //other.transform.position = new Vector2(ox, oy);

            int tx = Mathf.RoundToInt(gameObject.transform.position.x);
            int ty = Mathf.RoundToInt(gameObject.transform.position.y);

            //gameObject.transform.position = new Vector2(tx, ty);

            if (ox - tx == 0 && oy > ty)
            {
                colObj[0] = other.gameObject;
            }

            else if (ox - tx == 0 && oy < ty)
            {
                colObj[1] = other.gameObject;
            }

            else if (oy - ty == 0 && ox < tx)
            {
                colObj[2] = other.gameObject;
            }

            else if (oy - ty == 0 && ox > tx)
            {
                colObj[3] = other.gameObject;
            }
      }

        if (other.gameObject.CompareTag("player") && IsClone == 1)
        { 
            if (other.transform.position == this.gameObject.transform.position)
            {
                gameObject.SetActive(false);
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

    private void IsCollide(int i)
    {
        if (colObj[i] != null)
        {
            if (gameObject.CompareTag("player"))
            {
                if (!colObj[i].tag.Contains("player") && !colObj[i].tag.Contains("block"))
                {
                    if (colObj[i].GetComponent<CollisionManager>().IsPushed[i] == true) IsPlayerMoved[i] = true;
                    else IsPlayerMoved[i] = false;
                }

                else if (colObj[i].tag.Contains("block"))
                {
                    IsPlayerMoved[i] = false;
                }

                else if (colObj[i].tag.Contains("player"))
                {
                    PlayerController playerController = colObj[i].GetComponent<PlayerController>();
                    if (playerController.enabled && playerController.IsPlayerMoved[i] == true) IsPlayerMoved[i] = true;
                    else IsPlayerMoved[i] = false;
                }
            }

            else if (gameObject.CompareTag("robotplayer"))
            {
                if (!colObj[i].tag.Contains("player"))
                {
                    if (colObj[i].GetComponent<CollisionManager>().IsPushed[i] == true) IsPlayerMoved[i] = true;
                    else IsPlayerMoved[i] = false;
                }

                else if (colObj[i].tag.Contains("player"))
                {
                    PlayerController playerController = colObj[i].GetComponent<PlayerController>();
                    if (playerController.enabled && playerController.IsPlayerMoved[i] == true) IsPlayerMoved[i] = true;
                    else IsPlayerMoved[i] = false;
                }
            }
        }

        else if (colObj[i] == null) IsPlayerMoved[i] = true;
    }
}
