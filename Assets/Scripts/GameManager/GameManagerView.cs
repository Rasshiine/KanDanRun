using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Prime31.TransitionKit;

public class GameManagerView : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button rankingButton;

    [SerializeField] private Canvas valueCanvas;

    [SerializeField] Texture2D texture;

    [SerializeField] private Transform weatherTransform;
    private Vector3 rotateVector = new Vector3(0, 0, -180);

    public event Action StartScene;
    public event Func<int> GetScore;

    private void Awake()
    {
        startButton.onClick.AddListener(() => StartButton());
        creditButton.onClick.AddListener(() => CreditButton());
        howToPlayButton.onClick.AddListener(() => HowToPlayButton());
        titleButton.onClick.AddListener(() => ReloadScene());
        rankingButton.onClick.AddListener(() => RankingButton());
    }

    private void Start()
    {
        valueCanvas.gameObject.SetActive(false);

        startButton.gameObject.SetActive(true);
        creditButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(false);
        rankingButton.gameObject.SetActive(false);
    }

    

    void StartButton()
    {
        valueCanvas.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        StartScene?.Invoke();
    }

    void CreditButton()
    {

    }

    void HowToPlayButton()
    {

    }

    public void ReloadScene()
    {
        var pixelater = new ImageMaskTransition()
        {
            nextScene = 0,
            maskTexture = texture,
            backgroundColor = Color.green,
            //backgroundColor = new Color(120, 241, 83),
            //finalScaleEffect = PixelateTransition.PixelateFinalScaleEffect.ToPoint,
            duration = 1.0f
        };
        TransitionKit.instance.transitionWithDelegate(pixelater);

    }

    void RankingButton()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GetScore());
    }


    public void ShowScore(float score)
    {
        scoreText.text = score.ToString("f0");
    }

    public void ActivateUIs()
    {
        titleButton.gameObject.SetActive(true);
        rankingButton.gameObject.SetActive(true);
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
    }
}
