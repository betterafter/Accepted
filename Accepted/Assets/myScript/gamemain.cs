using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemain : MonoBehaviour
{
   
    public void OnStartClick()
    {
        SceneManager.LoadScene("stageSelect");
    }

    public void OnMapMakeClick()
    {
        SceneManager.LoadScene("MapCreater");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
