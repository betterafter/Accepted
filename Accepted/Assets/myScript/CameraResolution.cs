using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraResolution : MonoBehaviour
{
    Color color;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        if (SceneManager.GetActiveScene().name != "game") color = new Color(24 / 255f, 26 / 255f, 29 / 255f);
        else color = Color.black;

        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, color);
}