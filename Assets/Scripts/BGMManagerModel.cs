using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManagerModel : MonoBehaviour
{
    private static BGMManagerModel inst;
    private AudioSource audioSource;
    public static float bGMpitch = 1;

    private void Awake()
    {
        Debug.Log(inst);
        if (inst != null)
        {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1;
        bGMpitch = audioSource.pitch;
    }

    public void IncreasePitch()
    {
        audioSource.pitch += 0.025f;
        bGMpitch = audioSource.pitch;
    }
    public float GetPitch() => audioSource.pitch;
}
