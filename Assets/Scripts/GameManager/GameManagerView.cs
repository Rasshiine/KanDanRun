using System;
using DG.Tweening;
using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerView : MonoBehaviour
{
    [SerializeField] private RectTransform HPBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button titleBackButton;
    [SerializeField]

    private Button[] openingButtons;
    private Button[] endingButtons;
    Tween[] beatTweens;

    public static readonly float DefaultPitch = 60f / (160f * 2);

    [SerializeField] private Canvas valueCanvas;

    [SerializeField] Texture2D texture;

    [SerializeField] private Transform weatherTransform;
    private Vector3 rotateVector = new Vector3(0, 0, -180);

    public event Action SE_StartButton;
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
        titleBackButton.onClick.AddListener(() => TitleBackButton());

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

    private void Start()
    {
        if(!TutorialManager.isTutorialMode)
        valueCanvas.gameObject.SetActive(false);

        startButton.gameObject.SetActive(true);
        creditButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(false);
        rankingButton.gameObject.SetActive(false);

        titleBackButton.gameObject.SetActive(SceneManager.GetActiveScene().name != NameKeys.mainScene);
    }

    public Tween BeatAnimation(GameObject g, float pitch)
    {
        return g.transform.DOScale(0.2f, pitch)
            .SetRelative(true)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void StartButton()
    {
        valueCanvas.gameObject.SetActive(true);
        MoveButtons();
        SE_StartButton?.Invoke();
        StartScene?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ReloadScene();
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            .SetRelative(true)
            ;
        }
    }

    void CreditButton()
    {
        SE_StartButton?.Invoke();

    }

    void HowToPlayButton()
    {
        SE_StartButton?.Invoke();
        SceneLoader.inst.LoadScene(NameKeys.tutorialScene);
    }

    void TitleBackButton()
    {
        //canvasを作って中にボタン二つ入れる
        
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
        scoreText.text = score.ToString("f3");
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
