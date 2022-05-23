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
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1;
        bGMpitch = audioSource.pitch;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void IncreasePitch()
    {
        audioSource.pitch += 0.02f;
        bGMpitch = audioSource.pitch;
    }
    public float GetPitch() => audioSource.pitch;
}
