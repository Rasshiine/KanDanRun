using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.TransitionKit;

public class GameManagerModel : MonoBehaviour
{
    //public static bool isGamePlaying = false;
    private int level = 0;
    private float levelUpInterval = 100;
    private float magnification = 0.2f;
    private float score = 0;
    private float defaultPlayerSpeed = 3;
    private float playerSpeed = 3;
    private float probabilityOfChangeWeather = 0.3f;

    private bool isOutsideWarm;

    public enum GameState
    {
        notStarted,
        Playing,
        Finished
    }

    public static GameState currentState;

    [SerializeField] Texture2D texture;
    [SerializeField] Canvas valueCanvas;

    public event Action<bool> ChangeOutSideAirState;
    public event Action<float> ShowScore;
    public event Action<float> ChangeSpeed;
    public event Action ActivateUIs;
    public event Action ChangeWeather;
    public event Action<bool> ChangeWeatherWithNoMotion;

    void Awake()
    {
        ChangeSpeed?.Invoke(playerSpeed);
        currentState = GameState.notStarted;
    }

    // Start is called before the first frame update
    void Start()
    {
        //外気温を変化させる処理（ランダムか固定か）、背景変更の処理を書く
        isOutsideWarm = UnityEngine.Random.Range(0, 2) == 0;
        ChangeOutSideAirState?.Invoke(isOutsideWarm);
        ChangeWeatherWithNoMotion?.Invoke(isOutsideWarm);
        valueCanvas.gameObject.SetActive(false);
    }

    //ゲームスタートの処理
    public void StartScene(GameObject startButton)
    {
        currentState = GameState.Playing;
        //isGamePlaying = true;
        startButton.SetActive(false);
        valueCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeWeather?.Invoke();
        }
        if (currentState != GameState.Playing) return;
        //if (!isGamePlaying) return;

        //スコア加算
        score += playerSpeed * Time.deltaTime;
        ShowScore?.Invoke(score);

        //レベル上昇
        if(score > levelUpInterval * (level + 1))
        {
            level++;
            playerSpeed = defaultPlayerSpeed * (1 + magnification * level);
            ChangeSpeed?.Invoke(playerSpeed);
            if (UnityEngine.Random.Range(0.0f, 1.0f) < probabilityOfChangeWeather)
            {
                ChangeOutSideAirState?.Invoke(!isOutsideWarm);
                ChangeWeather?.Invoke();
            }
        }        
    }

    public void GameOver()
    {
        currentState = GameState.Finished;
        //isGamePlaying = false;
        ActivateUIs?.Invoke();
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
}
