using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{

    public string stageName;
    public bool IsRestart;
    public Stack<UndoItem> stack = new Stack<UndoItem>();

    private GameObject player, obj;
    private Vector3 playerpos, objpos;






    void Awake()
    {
        //Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, true);
        //Screen.SetResolution(720, 1280, true);

        //재시작 버튼 관련 
        IsRestart = false;

        //게임의 여러 기능 관리 
        GameObject gamemanger = GameObject.Find("GameManager");
        DontDestroyOnLoad(gamemanger);

        //멀티터치가 발생하면 벽돌을 뚫고 가는 현상이 있음 
        Input.multiTouchEnabled = false;

    }

    public void RestartClick()
    {
        SceneManager.LoadScene("game");
        IsRestart = true;
    }

    public void UndoClick()
    {
        if (stack.Count > 0)
        {
            player = stack.Peek().getPlayer();
            playerpos = stack.Peek().getPlayerpos();

            if (player != null)
            {
                player.transform.position = playerpos;
            }

            if (stack.Peek().getObj() != null)
            {
                obj = stack.Peek().getObj();
                objpos = stack.Peek().getObjpos();

                obj.transform.position = objpos;
            }

            stack.Pop();
        }
    }




}
