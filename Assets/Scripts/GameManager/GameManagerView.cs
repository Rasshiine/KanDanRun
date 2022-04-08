using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManagerView : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button startButton;

    [SerializeField] private Transform weatherTransform;
    private Vector3 rotateVector = new Vector3(0, 0, -180);

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

    public void ChangeWeather()
    {
        weatherTransform.DOLocalRotate(rotateVector, 1f)
            .SetEase(Ease.InOutBack)
            .SetRelative(true);
    }

    public void ChangeWeatherWithNoMotion(bool isWarm)
    {
        weatherTransform.rotation = Quaternion.Euler(0, 0, isWarm ? 180 : 0);
        Debug.Log(isWarm);
    }
}
