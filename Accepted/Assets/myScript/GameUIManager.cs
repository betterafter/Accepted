using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject QuitUI, StageQuitUI, SoundButton, SoundEffectButton;
    public Sprite SoundOn, SoundOff, SoundEffectOn, SoundEffectOff;
    public bool isSoundOn, isSoundEffectOn;

    private void Start()
    {
        QuitUI = gameObject.transform.GetChild(0).gameObject;
        StageQuitUI = gameObject.transform.GetChild(1).gameObject;
        isSoundOn = true; isSoundEffectOn = true;
        StartCoroutine("BackButton");
    }

    IEnumerator BackButton()
    {
        while (true)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (SceneManager.GetActiveScene().name != "game")
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        QuitUI.SetActive(true);
                    }
                }
                else if (SceneManager.GetActiveScene().name == "game")
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        StageQuitUI.SetActive(true);
                    }
                }
            }

            yield return null;
        }
    }

    public void Setting()
    {
        StageQuitUI.SetActive(true);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(1);
        StageQuitUI.SetActive(false);
    }

    public void BackgroundSoundActive()
    {
        if (isSoundOn)
        {
            isSoundOn = false;
            SoundButton.GetComponent<Image>().sprite = SoundOff;
            this.gameObject.GetComponent<AudioSource>().Stop();
        }
        else if (!isSoundOn)
        {
            isSoundOn = true;
            SoundButton.GetComponent<Image>().sprite = SoundOn;
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void SoundEffectActive()
    {
        if (isSoundEffectOn)
        {
            isSoundEffectOn = false;
            SoundEffectButton.GetComponent<Image>().sprite = SoundEffectOff;
        }
        else if (!isSoundEffectOn)
        {
            isSoundEffectOn = true;
            SoundEffectButton.GetComponent<Image>().sprite = SoundEffectOn;
        }
    }

    public void Close()
    {
        StageQuitUI.SetActive(false);
        QuitUI.SetActive(false);
    }

    public void QuitClick()
    {
        if (SceneManager.GetActiveScene().name != "game")
            Application.Quit();
        else
        {
            SceneManager.LoadScene(1);
            StageQuitUI.SetActive(false);
        }

    }

    public void NotQuitClick()
    {
        QuitUI.SetActive(false);
        StageQuitUI.SetActive(false);
    }
}
