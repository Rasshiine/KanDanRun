using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button startButton;

    public event Action ReloadScene;
    public event Action<GameObject> StartScene;

    private void Awake()
    {
        titleButton.onClick.AddListener(() => ReloadScene?.Invoke());
        titleButton.gameObject.SetActive(false);
        startButton.onClick.AddListener(() => StartScene?.Invoke(startButton.gameObject));
        startButton.gameObject.SetActive(true);
    }

    public void ShowScore(float score)
    {
        scoreText.text = score.ToString("f0");
    }

    public void ActivateUIs()
    {
        titleButton.gameObject.SetActive(true);
    }

}
