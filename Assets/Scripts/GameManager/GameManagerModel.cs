using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.TransitionKit;

public class GameManagerModel : MonoBehaviour
{
    public static bool isGameStarted = false;
    private int level = 0;
    private float levelUpInterval = 100;
    private float magnification = 0.2f;
    private float score = 0;
    private float defaultPlayerSpeed = 3;
    private float playerSpeed = 3;

    private bool isOutsideWarm;

    [SerializeField] Texture2D texture;
    [SerializeField] Canvas valueCanvas;

    public event Action<bool> ChangeOutSideAirState;
    public event Action<float> ShowScore;
    public event Action<float> ChangeSpeed;
    public event Action ActivateUIs;

    void Awake()
    {
        ChangeSpeed?.Invoke(playerSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        //外気温を変化させる処理（ランダムか固定か）、背景変更の処理を書く
        ChangeOutSideAirState?.Invoke(isOutsideWarm);

        valueCanvas.gameObject.SetActive(false);
    }

    //ゲームスタートの処理
    public void StartScene(GameObject startButton)
    {
        isGameStarted = true;
        startButton.SetActive(false);
        valueCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted) return;

        //スコア加算
        score += playerSpeed * Time.deltaTime;
        ShowScore?.Invoke(score);

        //レベル上昇
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
        isGameStarted = false;
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
