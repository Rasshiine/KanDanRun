using System;
using DG.Tweening;
using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerView : MonoBehaviour
{
    [SerializeField] private Image title;
    [SerializeField] private RectTransform HPBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Button titleBackButton;
    [SerializeField] private Button unPauseButton;

    private Button[] openingButtons;
    private Button[] endingButtons;
    Tween[] beatTweens;

    public static readonly float DefaultPitch = 60f / (160f * 2);

    [SerializeField] private Canvas valueCanvas;

    [SerializeField] Texture2D texture;

    [SerializeField] private Transform weatherTransform;
    private Vector3 rotateVector = new Vector3(0, 0, -180);

    public event Action SE_StartButton;
    public event Action SE_BackButton;
    public event Action StartScene;
    public event Func<int> GetScore;
    public event Func<float> GetPitch;

    private void Awake()
    {
        startButton.onClick.AddListener(() => StartButton());
        creditButton.onClick.AddListener(() => CreditButton());
        howToPlayButton.onClick.AddListener(() => HowToPlayButton());
        titleButton.onClick.AddListener(() => TitleBackButton());
        rankingButton.onClick.AddListener(() => RankingButton());
        pauseButton.onClick.AddListener(() => PauseButton(true));
        titleBackButton.onClick.AddListener(() => TitleBackButton());
        unPauseButton.onClick.AddListener(() => PauseButton(false));

        openingButtons = new Button[]
        { startButton, creditButton, howToPlayButton };
        endingButtons = new Button[]
        { titleButton,rankingButton };


        beatTweens = new Tween[openingButtons.Length];
        for (int i = 0; i < beatTweens.Length; i++)
        {
            beatTweens[i] = BeatAnimation(openingButtons[i].gameObject, DefaultPitch);
        }
    }

    void StartButton()
    {
        valueCanvas.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        title.DOFade(0, 0.2f);
        MoveButtons();
        SE_StartButton?.Invoke();
        StartScene?.Invoke();
    }

    void CreditButton()
    {
        SE_StartButton?.Invoke();
        SceneLoader.inst.LoadScene(NameKeys.creditScene);
    }

    void HowToPlayButton()
    {
        SE_StartButton?.Invoke();
        SceneLoader.inst.LoadScene(NameKeys.tutorialScene);
    }

    void RankingButton()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GetScore());
    }

    void PauseButton(bool paused)
    {
        if (paused) SE_StartButton?.Invoke();
        else SE_BackButton?.Invoke();
        Time.timeScale = paused ? 0 : 1;
        pauseCanvas.gameObject.SetActive(paused);
    }

    void TitleBackButton()
    {
        SE_StartButton?.Invoke();

        Time.timeScale = 1;
        SceneLoader.inst.LoadScene(NameKeys.mainScene);
    }

    
    private void Start()
    {
        if(!TutorialManager.isTutorialMode)
        valueCanvas.gameObject.SetActive(false);

        //startButton.gameObject.SetActive(true);
        //creditButton.gameObject.SetActive(true);
        //howToPlayButton.gameObject.SetActive(true);
        //titleButton.gameObject.SetActive(false);
        //rankingButton.gameObject.SetActive(false);
        //pauseButton.gameObject.SetActive(false);
        //pauseCanvas.gameObject.SetActive(false);

//        titleBackButton.gameObject.SetActive(SceneManager.GetActiveScene().name != NameKeys.mainScene);
    }

    public Tween BeatAnimation(GameObject g, float pitch)
    {
        return g.transform.DOScale(0.2f, pitch)
            .SetRelative(true)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void MoveButtons()
    {
        startButton.gameObject.GetComponent<RectTransform>();
        //よくみたらOpeninguButtonsあったわ
        RectTransform[] b = new RectTransform[]
        {
            startButton.gameObject.GetComponent<RectTransform>(),
            creditButton.gameObject.GetComponent<RectTransform>(),
            howToPlayButton.gameObject.GetComponent<RectTransform>()
        };
        for (int i = 0; i < beatTweens.Length; i++)
            beatTweens[i].Kill();


        for (int i = 0; i < b.Length; i++)
        {
            DOTween.Sequence()
            .Join(b[i].DOMoveX(-100f, 0.2f).SetEase(Ease.OutExpo))

            .Join(b[i].DOScaleX(-0.5f, 0.4f).SetEase(Ease.OutExpo))
            .Join(b[i].DORotate(new Vector3(0, 0, -10), 0.2f))
            //.AppendInterval(0.1f)
            //.AppendCallback(() => Debug.Break())
            .Append(b[i].DOMoveX(1200f, 0.3f).SetEase(Ease.InQuint))
            .Join(b[i].DOScaleX(0.5f, 0.2f).SetEase(Ease.InExpo))
            .Join(b[i].DORotate(new Vector3(0, 0, 10), 0.3f).SetEase(Ease.InOutCubic))
            .SetRelative(true);
        }
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
