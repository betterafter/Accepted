using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{

    public string stageName;
    public int stageLevel;
    public int dir;

    public bool IsRestart;
    public bool IsUndo;
    public bool IsLastClickedButton_Undo;
    public bool IsLastClickButton_Move;
    //public Stack<UndoItem> stack = new Stack<UndoItem>();

    private GameObject player, obj;
    private Vector3 playerpos, objpos;


    private void Start()
    {
        IsUndo = false;
    }

    void Awake()
    {
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
        IsUndo = true;
        IsLastClickedButton_Undo = true;
    }


}
