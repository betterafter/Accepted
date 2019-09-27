using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class mapMakerplugin : MonoBehaviour
{
    private Vector3 MousePosition;
    private bool IsErase;
    private bool IsMake;
    private string[ , ] map = new string[20, 15];
    private string CurrObject;
    public InputField StageName;
    public InputField chapterTitle;


    private void Start()
    {
        StartCoroutine("Editor");
        for(int y = 0; y < 20; y++)
        {
            for(int x = 0; x < 15; x++)
            {
                map[y, x] = "x";
            }
        }
    }

    private IEnumerator Editor()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))

            {
                MousePosition = Input.mousePosition;
                MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

                RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, 15f);

                if (hit) {
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

                                Debug.Log(x + ", " + y + ", " + CurrObject);
                                obj.tag = CurrObject;
                                map[y + 5, x + 7] = CurrObject;

                            }
                        }

                        else
                        {
                            if(!IsMake && IsErase)
                            {
                                Debug.Log(hit.collider.gameObject);

                                int x = (int)hit.collider.gameObject.transform.position.x;
                                int y = (int)hit.collider.gameObject.transform.position.y;
                                map[y + 5, x + 7] = "x";

                                Debug.Log(x + ", " + y + ", " + map[y + 5, x + 7]);
                                Destroy(hit.collider.gameObject);

                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }

    public void save()
    {

        if(StageName.text != "" && chapterTitle.text != "")
        {
            string path = "Assets/resources/map/";
            StreamWriter sw = new StreamWriter(path + "stage" + StageName.text + ".txt");
            for(int y = 0; y < 20; y++)
            {
                for(int x = 0; x < 15; x++)
                {
                    Debug.Log(x + ", " + y + ", " + map[y, x]);
                    if(map[y, x] != "x")
                    {
                        int trueX = x - 7;
                        int trueY = y - 5;
                        sw.Write(map[y, x] + "," + trueX + "," + trueY);
                        sw.Write("\n");
                    }
                }
            }
            sw.Write("chapter " + StageName.text + "," + chapterTitle.text);
            sw.Close();
        }
    }
}
