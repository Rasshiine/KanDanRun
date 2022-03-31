using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMaker : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject face;
    GameObject[] faceArray;

    private float width = 1280;
    private float height = 720;

    private int num_H = 5;
    private int num_V = 3;


    private void Awake()
    {
        faceArray = new GameObject[num_H * num_V];
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < num_H; i++)
        {
            for(int j = 0; j < num_V; j++)
            {
                faceArray[i * 3 + j] =
                    Instantiate(face, new Vector3
                    ((width / num_H) * i - width / 2,
                    -(height / num_V) * j + height / 2,
                    0),
                    Quaternion.identity);
                faceArray[i * 3 + j].transform.SetParent(canvas.transform,false);
            }
        }
    }
}
