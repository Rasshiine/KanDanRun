using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManagerModel : MonoBehaviour
{
    public static BGMManagerModel inst;
    private AudioSource audioSource;
    public static float bGMpitch = 1;

    private void Awake()
    {
        //Debug.Log(inst);
        //if (inst != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //inst = this;
        //Debug.Log(inst);

        //DontDestroyOnLoad(inst);
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1;
        bGMpitch = audioSource.pitch;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //Debug.Log("call");
    }

    public void IncreasePitch()
    {
        audioSource.pitch += 0.02f;
        bGMpitch = audioSource.pitch;
    }
    public float GetPitch() => audioSource.pitch;
}
