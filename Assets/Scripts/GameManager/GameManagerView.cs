using System;
using DG.Tweening;
using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button rankingButton;

    private Button[] openingButtons;
    private Button[] endingButtons;
    public static readonly float DefaultPitch = 60f / (160f * 2);

    [SerializeField] private Canvas valueCanvas;

    [SerializeField] Texture2D texture;

    [SerializeField] private Transform weatherTransform;
    private Vector3 rotateVector = new Vector3(0, 0, -180);

    public event Action StartScene;
    public event Func<int> GetScore;
    public event Func<float> GetPitch;

    private void Awake()
    {
        startButton.onClick.AddListener(() => StartButton());
        creditButton.onClick.AddListener(() => CreditButton());
        howToPlayButton.onClick.AddListener(() => HowToPlayButton());
        titleButton.onClick.AddListener(() => ReloadScene());
        rankingButton.onClick.AddListener(() => RankingButton());

        openingButtons = new Button[]
        { startButton, creditButton, howToPlayButton };
        endingButtons = new Button[]
        { titleButton,rankingButton };
    }

    private void Start()
    {
        valueCanvas.gameObject.SetActive(false);

        startButton.gameObject.SetActive(true);
        creditButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(false);
        rankingButton.gameObject.SetActive(false);

        foreach (Button b in openingButtons)
        {
            BeatAnimation(b.gameObject, DefaultPitch);
        }
    }

    public void BeatAnimation(GameObject g, float pitch)
    {
        g.transform.DOScale(0.2f, pitch)
            .SetRelative(true)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo);
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
            duration = 0.3f,
            
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
        foreach (Button b in endingButtons)
            BeatAnimation(b.gameObject, DefaultPitch / GetPitch());
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
