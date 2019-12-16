using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepCollision : MonoBehaviour
{
    private Stage s;
    private Sprite StepOff, StepOn;
    AudioSource audioSource;
    public AudioClip StepOnSound;

    private void Awake()
    {
        StepOff = gameObject.GetComponent<SpriteRenderer>().sprite;
        StepOn = Resources.Load<Sprite>(gameObject.tag);

        audioSource = this.gameObject.GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            this.audioSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            s.currStepCnt++;
        }
        else if(gameObject.tag.Contains("left") && collision.gameObject.tag.Contains("left"))
        {
            this.audioSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
        }
        else if (gameObject.tag.Contains("right") && collision.gameObject.tag.Contains("right"))
        {
            this.audioSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
        }
        else if (gameObject.tag.Contains("up") && collision.gameObject.tag.Contains("up"))
        {
            this.audioSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
        }
        else if (gameObject.tag.Contains("down") && collision.gameObject.tag.Contains("down"))
        {
            this.audioSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOff;
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            s.currStepCnt--;
        }
        else if ((gameObject.tag.Contains("down") && collision.gameObject.tag.Contains("down")) ||
            (gameObject.tag.Contains("up") && collision.gameObject.tag.Contains("up")) ||
            (gameObject.tag.Contains("left") && collision.gameObject.tag.Contains("left")) ||
            (gameObject.tag.Contains("right") && collision.gameObject.tag.Contains("right")))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOff;
        }
        Debug.Log("???");
    }
}
