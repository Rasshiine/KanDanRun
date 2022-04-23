using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManagerModel : MonoBehaviour
{
    private static BGMManagerModel inst;
    private AudioSource audioSource;

    private void Awake()
    {
        if (inst != null)
        {
            Destroy(gameObject);
            return;
        }
        inst = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1;
    }

    public void IncreasePitch() => audioSource.pitch += 0.025f;
}
