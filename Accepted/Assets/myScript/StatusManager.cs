﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public int RotateAngle;

    private void Start()
    {
        RotateAngle = 0;
    }

    private void Update()
    {
        if ((int)this.transform.eulerAngles.z != 0)
        {
            if ((int)(this.transform.eulerAngles.z % 270) == 0)
            {
                RotateAngle = 270;
            }
            else if ((int)(this.transform.eulerAngles.z % 180) == 0)
            {
                RotateAngle = 180;
            }
            else if ((int)(this.transform.eulerAngles.z % 90) == 0)
            {
                RotateAngle = 90;
            }
            else
            {
                RotateAngle = 0;
            }
        }
        else
        {
            RotateAngle = 0;
        }
    }
}
