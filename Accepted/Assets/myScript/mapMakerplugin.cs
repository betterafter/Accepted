using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mapMakerplugin : MonoBehaviour
{
    private Vector3 MousePosition;
    private bool IsErase;
    private bool IsMake;
    private List<string>[ , ] map = new List<string>[20 , 15];
    private string CurrObject;
    public InputField StageName;
    public InputField chapterTitle;

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)

        {

            if (Input.GetKey(KeyCode.Escape))

            {
                SceneManager.LoadScene("main");
            }

        }
    }

    private void Start()
    {
        StartCoroutine("Editor");
        for(int y = 0; y < 20; y++)
        {
            for(int x = 0; x < 15; x++)
            {
                List<string> list = new List<string>();
                map[y, x] = list; 
                for(int z = 0; z < 2; z++)
                {
                    map[y, x].Add("x");
                }
            }
        }
    }



    private IEnumerator Editor()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))

                {
                    MousePosition = Input.mousePosition;
                    MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

                    RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, 15f);

                    if (hit)
                    {
                        if (hit.collider != null)

                        {
                            if (hit.collider.gameObject.CompareTag("eraser"))
                            {
                                IsMake = false;
                                IsErase = true;
                            }

                            else if (hit.collider.gameObject.CompareTag("maker"))
                            {
                                IsErase = false;
                                IsMake = true;
                                CurrObject = hit.collider.gameObject.name;
                            }

                            else if (hit.collider.gameObject.CompareTag("position"))
                            {
                                if (IsMake && !IsErase)
                                {
                                    string path = "Prefabs/mapIcon/" + CurrObject;

                                    int x = (int)hit.collider.gameObject.transform.position.x;
                                    int y = (int)hit.collider.gameObject.transform.position.y;

                                    GameObject o = Resources.Load(path) as GameObject;
                                    GameObject obj = Instantiate(o, new Vector3(x, y, 0), Quaternion.identity);
                                    obj.tag = CurrObject;

                                    map[y + 5, x + 7][0] = CurrObject;
                                }
                            }

                            else
                            {
                                if (IsMake && !IsErase)
                                {
                                    if (hit.collider.gameObject.tag != CurrObject)
                                    {
                                        string path = "Prefabs/mapIcon/" + CurrObject;

                                        int x = (int)hit.collider.gameObject.transform.position.x;
                                        int y = (int)hit.collider.gameObject.transform.position.y;


                                        GameObject o = Resources.Load(path) as GameObject;
                                        GameObject obj = Instantiate(o, new Vector3(x, y, 0), Quaternion.identity);

                                        obj.tag = CurrObject;

                                        if (map[y + 5, x + 7][1] == "x") map[y + 5, x + 7][1] = CurrObject;
                                    }
                                }

                                if (!IsMake && IsErase)
                                {
                                    int x = (int)hit.collider.gameObject.transform.position.x;
                                    int y = (int)hit.collider.gameObject.transform.position.y;

                                    if (map[y + 5, x + 7][0] == hit.collider.gameObject.tag) map[y + 5, x + 7][0] = "x";
                                    else map[y + 5, x + 7][1] = "x";

                                    Destroy(hit.collider.gameObject);

                                }
                            }
                        }
                    }
                }
                yield return null;
            }
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            while (true) {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                        if (hit)
                        {
                            if (hit.collider != null)

                            {
                                if (hit.collider.gameObject.CompareTag("eraser"))
                                {
                                    Debug.Log("eraser");
                                    IsMake = false;
                                    IsErase = true;
                                }

                                else if (hit.collider.gameObject.CompareTag("maker"))
                                {
                                    Debug.Log("maker");
                                    IsErase = false;
                                    IsMake = true;
                                    CurrObject = hit.collider.gameObject.name;
                                }

                                else if (hit.collider.gameObject.CompareTag("position"))
                                {
                                    Debug.Log("position");
                                    if (IsMake && !IsErase)
                                    {
                                        string path = "Prefabs/mapIcon/" + CurrObject;

                                        int x = (int)hit.collider.gameObject.transform.position.x;
                                        int y = (int)hit.collider.gameObject.transform.position.y;

                                        GameObject o = Resources.Load(path) as GameObject;
                                        GameObject obj = Instantiate(o, new Vector3(x, y, 0), Quaternion.identity);
                                        obj.tag = CurrObject;

                                        map[y + 5, x + 7][0] = CurrObject;
                                    }
                                }

                                else
                                {
                                    Debug.Log("others");
                                    if (IsMake && !IsErase)
                                    {
                                        if (hit.collider.gameObject.tag != CurrObject)
                                        {
                                            string path = "Prefabs/mapIcon/" + CurrObject;

                                            int x = (int)hit.collider.gameObject.transform.position.x;
                                            int y = (int)hit.collider.gameObject.transform.position.y;


                                            GameObject o = Resources.Load(path) as GameObject;
                                            GameObject obj = Instantiate(o, new Vector3(x, y, 0), Quaternion.identity);

                                            obj.tag = CurrObject;

                                            if (map[y + 5, x + 7][1] == "x") map[y + 5, x + 7][1] = CurrObject;
                                        }
                                    }

                                    if (!IsMake && IsErase)
                                    {
                                        int x = (int)hit.collider.gameObject.transform.position.x;
                                        int y = (int)hit.collider.gameObject.transform.position.y;

                                        if (map[y + 5, x + 7][0] == hit.collider.gameObject.tag) map[y + 5, x + 7][0] = "x";
                                        else map[y + 5, x + 7][1] = "x";

                                        Destroy(hit.collider.gameObject);

                                    }
                                }
                            }
                        }
                    }
                }
                yield return null;
            }
        }

    }

    public void save()
    {

        if(StageName.text != "" && chapterTitle.text != "")
        {
            string path;
            if (Application.platform != RuntimePlatform.Android)
            {
                path = "Assets/resources/map/";
            }
            else
            {
                path = Application.persistentDataPath;
            }

            StreamWriter sw = new StreamWriter(path + "stage" + StageName.text + ".txt");
            for(int y = 0; y < 20; y++)
            {
                for(int x = 0; x < 15; x++)
                {
                    Debug.Log(x + ", " + y + ", " + map[y, x]);
                    for(int z = 0; z < 2; z++)
                    {
                        if (map[y, x][z] != "x")
                        {
                            int trueX = x - 7;
                            int trueY = y - 5;
                            sw.Write(map[y, x][z] + "," + trueX + "," + trueY);
                            sw.Write("\n");
                        }
                    }
                }
            }
            sw.Write("chapter " + StageName.text + "," + chapterTitle.text);
            sw.Close();
        }
    }

}
