using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : CollisionManager
{
    public detection Detection;
    float MoveXTime, MoveYTime;


    public override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void Update()
    {
        base.Update();


        if(Detection.TargetObject != null)
        {
            int TargetX = (int)Detection.TargetObject.transform.position.x;
            int TargetY = (int)Detection.TargetObject.transform.position.y;
            int ThisX = (int)this.gameObject.transform.position.x;
            int ThisY = (int)this.gameObject.transform.position.y;

            if (MoveXTime >= 2f)
            {
                if (Mathf.Abs(TargetX - ThisX) != 0)
                {
                    Debug.Log("MoveX");
                    if (ThisX < TargetX && (ColObj[3] == null || ColObj[3].CompareTag("player")))
                    {
                        Vector3 ThisPosition = new Vector3(ThisX + 1, ThisY, this.gameObject.transform.position.z);
                        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, ThisPosition, 1);
                    }
                    else if (ThisX > TargetX && (ColObj[2] == null || ColObj[2].CompareTag("player")))
                    {
                        Vector3 ThisPosition = new Vector3(ThisX - 1, ThisY, this.gameObject.transform.position.z);
                        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, ThisPosition, 1);
                    }
                }

                MoveXTime = 0.0f;
            }

           
            if(MoveYTime >= 2f)
            {
                if (Mathf.Abs(TargetY - ThisY) != 0)
                {
                    Debug.Log("MoveY");
                    if (ThisY < TargetY && (ColObj[0] == null || ColObj[0].CompareTag("player")))
                    {
                        Vector3 ThisPosition = new Vector3(ThisX, ThisY + 1, this.gameObject.transform.position.z);
                        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, ThisPosition, 1);
                    }
                    else if (ThisY > TargetY && (ColObj[1] == null || ColObj[1].CompareTag("player")))
                    {
                        Vector3 ThisPosition = new Vector3(ThisX, ThisY - 1, this.gameObject.transform.position.z);
                        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, ThisPosition, 1);
                    }
                }

                MoveYTime = 0.0f;
            }

            MoveXTime += 0.1f; MoveYTime += 0.1f;


            if(ThisX == TargetX && ThisY == TargetY)
            {

            }
        }
    }





    public override void Start()
    {
        base.Start();
        MoveXTime = 2.0f; MoveYTime = 0.0f;

    }
}
