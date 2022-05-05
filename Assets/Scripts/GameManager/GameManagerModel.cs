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
    private float magnification = 0.15f;
    private float score = 0;
    private float defaultPlayerSpeed = 5;
    private float playerSpeed = 0;
    private float probabilityOfChangeWeather = 1f;

    public static bool isOutsideWarm;

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
    public event Action ChangeWeather;
    public event Action<float> ChangeBGColor;
    public event Action<bool> ChangeWeatherWithNoMotion;
    public event Action SE_ChangeWeather;
    public event Action SE_LevelUp;
    public event Action SE_GameOver;
    public event Action IncreasePitch;

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
        isOutsideWarm = UnityEngine.Random.Range(0, 2) == 0;
        ChangeOutSideAirState?.Invoke(isOutsideWarm);
        ChangeWeatherWithNoMotion?.Invoke(isOutsideWarm);
    }

    //ゲームスタートの処理
    public void StartScene()
    {
        currentState = GameState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != GameState.Playing) return;

        //スコア加算
        score += playerSpeed * Time.deltaTime;
        ShowScore?.Invoke(score);

        //レベル上昇
        if (score > levelUpInterval * (level + 1))
        {
            level++;
            playerSpeed = defaultPlayerSpeed * (1 + magnification * level);
            ChangeSpeed?.Invoke(playerSpeed);
            IncreasePitch?.Invoke();
            if (UnityEngine.Random.Range(0.0f, 1.0f) < probabilityOfChangeWeather)
            {
                isOutsideWarm = !isOutsideWarm;
                ChangeOutSideAirState?.Invoke(isOutsideWarm);
                ChangeWeather?.Invoke();
                ChangeBGColor?.Invoke(1);
            }
        }
    }

    public void GameOver()
    {
        currentState = GameState.Finished;
        ActivateUIs?.Invoke();
        SE_GameOver?.Invoke();
    }

    public int GetScore() => (int)score;
    public bool GetIsOutsideWarm() => isOutsideWarm;
}