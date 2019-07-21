using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelect : MonoBehaviour
{
  
    private void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { this.GetComponent<StageSelect>().StageClick(); });
    }


    public void StageClick()
    {
        sceneManager s = GameObject.Find("GameManager").GetComponent<sceneManager>();
        s.stageName = this.gameObject.name;
        s.IsRestart = false;

        SceneManager.LoadScene("game");
    }
}
