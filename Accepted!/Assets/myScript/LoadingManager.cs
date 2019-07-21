using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    GameObject[] load = new GameObject[10];
    public Sprite LoadcountSprite;
    

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            load[i] = GameObject.Find((i + 1).ToString());
        }
        StartCoroutine("LoadScene");
    }


    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation Aop = SceneManager.LoadSceneAsync("main");
        Aop.allowSceneActivation = false;
        int i = 1;

        while (!Aop.isDone)
        {
            yield return null;

            if(Aop.progress >= 0.9f)
            {
                load[i].GetComponent<SpriteRenderer>().sprite = LoadcountSprite;
                Aop.allowSceneActivation = true;
            }

            else if(Aop.progress >= i * 0.1f)
            {
                load[i].GetComponent<SpriteRenderer>().sprite = LoadcountSprite;
                i++;
            }
        }

    }

}
