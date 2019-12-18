using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemain : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }


    public void OnStartClick()
    {
        audioSource.Play();
        SceneManager.LoadScene("stageSelect");
    }

    public void OnMapMakeClick()
    {
        audioSource.Play();
        SceneManager.LoadScene("MapCreater");
    }

    public void OnGoogleLoginClick()
    {
        audioSource.Play();
    }

    public void OnOptionClick()
    {
        audioSource.Play();
    }

    public void OnQuitClick()
    {
        audioSource.Play();
        Application.Quit();
    }
}
