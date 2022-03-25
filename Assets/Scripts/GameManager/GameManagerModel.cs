using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerModel : MonoBehaviour
{
    public static bool isGameStarted = true;
    private int level = 0;
    private float levelUpInterval = 100;
    private float magnification = 0.2f;
    private float score = 0;
    private float defaultPlayerSpeed = 3;
    private float playerSpeed = 3;

    private bool isOutsideWarm;

    public event Action<bool> ChangeOutSideAirState;
    public event Action<float> ShowScore;
    public event Action<float> ChangeSpeed;

    void Awake()
    {
        ChangeSpeed?.Invoke(playerSpeed);
    }
    // Start is called before the first frame update
    void Start()
    {
        //外気温を変化させる処理（ランダムか固定か）、背景変更の処理を書く
        ChangeOutSideAirState?.Invoke(isOutsideWarm);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted) return;
        score += playerSpeed * Time.deltaTime;
        ShowScore?.Invoke(score);

        if(score > levelUpInterval * (level + 1))
        {
            level++;
            playerSpeed = defaultPlayerSpeed * (1 + magnification * level);
            ChangeSpeed?.Invoke(playerSpeed);
        }
    }

    public void ChangeAirState()
    {

    }

    public void GameOver()
    {
        Debug.Break();
    }
}
