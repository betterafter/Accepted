using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepCollision : MonoBehaviour
{
    private Stage s;
    private Sprite StepOff, StepOn;
    AudioSource audioSource;

    public AudioClip AudioClip;

    public AudioClip StepOnSound;
    public bool isStep = false, brickStepOn = false, ArrowStepOn = false, RotateStepOn = false;

    sceneManager SceneManager;
    GameUIManager GameUIManager;
    bool isSwitch = false;

    GameObject child, GameManager;

    int PlayStepSound;

    private void Awake()
    {
        GameManager = GameObject.Find("GameManager");
        SceneManager = GameManager.GetComponent<sceneManager>();
        GameUIManager = GameManager.GetComponent<GameUIManager>();

        StepOff = gameObject.GetComponent<SpriteRenderer>().sprite;
        //StepOn = Resources.Load<Sprite>(gameObject.tag);

        audioSource = this.gameObject.GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
    }

    private void Start()
    {

        SceneManager.stepList.Add(this.gameObject);

        for(int i = 0; i < 10; i++)
        {
            if (SceneManager.StepName[i] != null && SceneManager.StepName[i] != "" && gameObject.CompareTag(SceneManager.StepName[i]))
            {
                StepOn = SceneManager.StepSprite[i]; break;
            }
        }

        for(int i = 0; i < 10; i++)
        {
            if (SceneManager.isSwitch[i] != null && SceneManager.isSwitch[i] != "" && gameObject.CompareTag(SceneManager.isSwitch[i]))
            {
                isSwitch = true; StepOn = SceneManager.StepSprite[i + 5]; break;
            }
        }
    }


    private void Update()
    {

        if (!isSwitch)
        {
            int y = (int)gameObject.transform.position.y, x = (int)gameObject.transform.position.x;

            if (x + 8 >= 0 && x + 8 <= 15 && SceneManager.minimap[y + 5, x + 8] != null &&
                 SceneManager.minimap[y + 5, x + 8].CompareTag("verbar") && SceneManager.minimap[y + 5, x + 8].GetComponent<CollisionManager>().isConnected)
            {
                isStep = true;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
                child.SetActive(true);
            }
            else if (x + 6 >= 0 && x + 6 <= 15 && SceneManager.minimap[y + 5, x + 6] != null &&
                 SceneManager.minimap[y + 5, x + 6].CompareTag("verbar") && SceneManager.minimap[y + 5, x + 6].GetComponent<CollisionManager>().isConnected)
            {
                isStep = true;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
                child.SetActive(true);
            }
            else if (y + 6 >= 0 && y + 6 <= 20 && SceneManager.minimap[y + 6, x + 7] != null &&
                 SceneManager.minimap[y + 6, x + 7].CompareTag("hobar") && SceneManager.minimap[y + 6, x + 7].GetComponent<CollisionManager>().isConnected)
            {
                isStep = true;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
                child.SetActive(true);
            }
            else if (y + 4 >= 0 && y + 4 <= 20 && SceneManager.minimap[y + 4, x + 7] != null &&
                 SceneManager.minimap[y + 4, x + 7].CompareTag("hobar") && SceneManager.minimap[y + 4, x + 7].GetComponent<CollisionManager>().isConnected)
            {
                isStep = true;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
                child.SetActive(true);
            }

            else Other();
        }

        else Other();
    }

    void Other()
    {
        if (brickStepOn)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            isStep = true;
        }

        else if (ArrowStepOn)
        {
            isStep = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
        }

        else if (RotateStepOn)
        {
            isStep = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }

        else
        {
            isStep = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOff;
            child.SetActive(false);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            if (GameUIManager.isSoundEffectOn) PlayStepSound = AndroidNativeAudio.play(SceneManager.StepSound[0]);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            brickStepOn = true;
        }
        else if (gameObject.tag.Contains("left") && collision.gameObject.tag.Contains("left"))
        {
            if (GameUIManager.isSoundEffectOn) PlayStepSound = AndroidNativeAudio.play(SceneManager.StepSound[0]);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            ArrowStepOn = true;
        }
        else if (gameObject.tag.Contains("right") && collision.gameObject.tag.Contains("right"))
        {
           if (GameUIManager.isSoundEffectOn) PlayStepSound = AndroidNativeAudio.play(SceneManager.StepSound[0]);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            ArrowStepOn = true;
        }
        else if (gameObject.tag.Contains("up") && collision.gameObject.tag.Contains("up"))
        {
            if (GameUIManager.isSoundEffectOn) PlayStepSound = AndroidNativeAudio.play(SceneManager.StepSound[0]);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            ArrowStepOn = true;
        }
        else if (gameObject.tag.Contains("down") && collision.gameObject.tag.Contains("down"))
        {
           if (GameUIManager.isSoundEffectOn) PlayStepSound = AndroidNativeAudio.play(SceneManager.StepSound[0]);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            ArrowStepOn = true;
        }

        if (gameObject.CompareTag("rotatestep"))
        {
            RotateStepOn = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            collision.GetComponent<CollisionManager>().isStepOn = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }
        else if(gameObject.tag.Contains("left") && collision.gameObject.tag.Contains("left"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }
        else if (gameObject.tag.Contains("right") && collision.gameObject.tag.Contains("right"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }
        else if (gameObject.tag.Contains("up") && collision.gameObject.tag.Contains("up"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }
        else if (gameObject.tag.Contains("down") && collision.gameObject.tag.Contains("down"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOn;
            child.SetActive(true);
        }
        if (gameObject.CompareTag("rotatestep"))
        {
            RotateStepOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.tag.Contains(collision.gameObject.tag))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOff;
            child.SetActive(false);
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            collision.GetComponent<CollisionManager>().isStepOn = false;
            //s.currStepCnt--;
            brickStepOn = false;
            //s.currStepCnt--;
        }
        else if ((gameObject.tag.Contains("down") && collision.gameObject.tag.Contains("down")) ||
            (gameObject.tag.Contains("up") && collision.gameObject.tag.Contains("up")) ||
            (gameObject.tag.Contains("left") && collision.gameObject.tag.Contains("left")) ||
            (gameObject.tag.Contains("right") && collision.gameObject.tag.Contains("right")))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = StepOff;
            s = GameObject.FindWithTag("accepted").GetComponent<Stage>();
            child.SetActive(false);
            ArrowStepOn = false;
            //s.currStepCnt--;
        }

        if (gameObject.CompareTag("rotatestep"))
        {
            RotateStepOn = false;
        }
    }
}
