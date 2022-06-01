using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.TransitionKit;

public class GameManagerModel : MonoBehaviour
{
    //public static bool isGamePlaying = false;
    private int level = 0;
    private float levelUpInterval = 80f;
    private float magnification = 1f;
    private float score = 0;
    private float defaultPlayerSpeed = 5f;
    [SerializeField]private float playerSpeed = 0;
    private float maxSpeedMagnification = 4;

    public static bool isOutsideWarm;

    public static bool wasRetryButtonPressed = false;

    public enum GameState
    {
        notStarted,
        Playing,
        Finished
    }

    public static GameState currentState;

    public event Action<bool> ChangeOutSideAirState;
    public event Action<float> ShowScore;
    public event Action<float> ChangeSpeed;
    public event Action ActivateUIs;
    public event Action<float> ChangeBGColor;
    public event Action<bool> ChangeWeatherWithNoMotion;
    public event Action SE_ChangeWeather;
    public event Action SE_LevelUp;
    public event Action SE_GameOver;
    public event Action IncreasePitch;
    public event Action GameStart;
    public event Action<bool> StartBlinking;

    void Awake()
    {
        currentState = GameState.notStarted;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = defaultPlayerSpeed;
        ChangeSpeed?.Invoke(playerSpeed);
        //外気温を変化させる処理（ランダムか固定か）、背景変更の処理を書く
        isOutsideWarm = TutorialManager.isTutorialMode ? true : UnityEngine.Random.Range(0, 2) == 0;
        ChangeOutSideAirState?.Invoke(isOutsideWarm);
        ChangeWeatherWithNoMotion?.Invoke(isOutsideWarm);

        if (wasRetryButtonPressed)
        {
            currentState = GameState.Playing;
            GameStart?.Invoke();
        }
    }

    //ゲームスタートの処理
    public void StartScene()
    {
        currentState = GameState.Playing;
    }

    float nextInterval = 100;

    void Update()
    {
        if (currentState != GameState.Playing) return;

        //スコア加算
        score += playerSpeed * Time.deltaTime;
        ShowScore?.Invoke(score);

        //レベル上昇
        if (score <= nextInterval) return;

        level++;
        nextInterval = levelUpInterval * (level + 1) * (1 + (float)level / 10) * UnityEngine.Random.Range(0.95f, 1.05f);
        StartBlinking?.Invoke(isOutsideWarm);

        if (playerSpeed > defaultPlayerSpeed * maxSpeedMagnification) return;
        playerSpeed += Magnification();
        Debug.Log(playerSpeed);
        ChangeSpeed?.Invoke(playerSpeed);
        IncreasePitch?.Invoke();
    }

    float Magnification()
    {
        return magnification * (level > 5 ? 0.3f : 1f);
    }

    public void _ChangeWeather()
    {
        isOutsideWarm = !isOutsideWarm;
        ChangeOutSideAirState?.Invoke(isOutsideWarm);
        ChangeBGColor?.Invoke(1);
    }
    public void GameOver()
    {
        currentState = GameState.Finished;
        ActivateUIs?.Invoke();
        SE_GameOver?.Invoke();
    }

    public int GetLevel() => level;
    public int GetScore() => (int)score;
    public bool GetIsOutsideWarm() => isOutsideWarm;
}