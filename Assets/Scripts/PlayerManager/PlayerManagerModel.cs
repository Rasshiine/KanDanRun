using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerManagerModel : MonoBehaviour
{
    private float playerHP = 1;
    private float damageSpeed = 0.5f;
    private float healingSpeed = 0.05f;
    private float healingInterval = 3.0f;
    private bool isPlayerWarm = false;
    private bool isOutsideWarm = false;
    private bool isStressed = false;

    private bool isExactlyTimerStarted = false;
    private bool isInvincible = false;
    private bool ClothChangedTheOneBefore = false;
    private float invincibleTimer = 0f;
    private float limitOfInvincibleTimer = 0.2f;
    private float exactlyTimer = 0f;
    private float limitOfExactlyTimer = 0.05f;

    public event Action<float> ShowHP;
    public event Action<bool> ChangeAnimation;
    public event Action GameOver;
    public event Action ChangeToGameOverAnimation;
    public event Action StartJustTimer;
    public event Action PlayJustSound;

    //家の中にいる間
    public void ChangeDamageState(bool? isHouseWarm)
    {
        //Debug.Log(isPlayerWarm == isHouseWarm);
        isStressed = isPlayerWarm == isHouseWarm;
    } 

    //家の外にいるとき
    public void CheckOutsideAir()
    {
        isStressed = isPlayerWarm == isOutsideWarm;
    }

    
    public void ChangeOutSideAirState(bool b)
    {
        isOutsideWarm = b;
        if (GameManagerModel.currentState != GameManagerModel.GameState.notStarted) return;
        if (TutorialManager.isTutorialMode) return;
        //初期設定
        ChangePlayerState(!isOutsideWarm);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;

        CountInvincibleTimer();

        CountExactlyTimer();
        
        if (isStressed && !isInvincible)
        {
            playerHP -= damageSpeed * Time.deltaTime;
            CancelInvoke(nameof(HealDamage));
            
            if (playerHP <= 0 && !TutorialManager.isTutorialMode)
            {
                GameOver?.Invoke();
                ChangeToGameOverAnimation?.Invoke();
            }
        }
        else
        {
            Invoke(nameof(HealDamage), healingInterval);
        }

        ShowHP?.Invoke(playerHP);

        invText.text = isInvincible.ToString();

        
    }
    [SerializeField] Text invText;


    void HealDamage()
    {
        playerHP += healingSpeed * Time.deltaTime;
        if (playerHP > 1) playerHP = 1;
    }

    void CountInvincibleTimer()
    {
        if (!isInvincible) return;
        isStressed = false;
        invincibleTimer += Time.deltaTime;

        if (invincibleTimer < limitOfInvincibleTimer) return;
        ResetInvincible();
    }

    public void ChangePlayerState(bool isWarm)
    {
        //ResetInvincible();
        if (isPlayerWarm == isWarm) return;
        isPlayerWarm = isWarm;
        ChangeAnimation?.Invoke(isPlayerWarm);

        if (isStressed) return;
        isInvincible = true;
    }

    void ResetInvincible()
    {
        isInvincible = false;
        invincibleTimer = 0;
    }

    //ストレスを受けていない && 外気と違う家に入る &&際に実行
    public void ChangeExactly(bool didChangedCloth, bool isWarm)
    {
        if (didChangedCloth)
        {
            if (isPlayerWarm == isWarm || ClothChangedTheOneBefore) return;
            ClothChangedTheOneBefore = true;
        }
        else
        {
            if (isOutsideWarm == isWarm) return;
            isInvincible = true;
        }

        if (isStressed && !isInvincible) return;

        if (!isExactlyTimerStarted)
        {
            isExactlyTimerStarted = true;
            return;
        }

        //CountExactlyTimer()でisExactlyTimerStartedを適宜falseにしているので、
        //↑が通るということはジャストなはず
        ClothChangedTheOneBefore = false;
        StartJustTimer?.Invoke();

        if (!TutorialManager.isTutorialMode || TutorialManager.progress == 9) 
        PlayJustSound?.Invoke();
        //playerHP += 0.05f;
        //ゲージの増加
        DOTween.To(() => playerHP, (n) => playerHP = n, playerHP + 0.05f, 0.25f).SetUpdate(true);
        if (playerHP > 1) playerHP = 1;

        if (TutorialManager.isTutorialMode) TutorialManager.SetProgress(10);
    }

    void CountExactlyTimer()
    {
        if (!isExactlyTimerStarted) return;
        exactlyTimer += Time.deltaTime;
        if (exactlyTimer > limitOfExactlyTimer)
        {
            ClothChangedTheOneBefore = false;
            exactlyTimer = 0;
            isExactlyTimerStarted = false;
        }
        //Debug.Log("到達");
    }

    public bool GetIsStressed() => isStressed;
    public bool GetisPlayerWarm() => isPlayerWarm;
    
    
}
