using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerModel : MonoBehaviour
{
    private float playerHP = 1;
    private float damageSpeed = 0.2f;
    private float healingSpeed = 0.05f;
    private float healingInterval = 1.0f;
    private bool isPlayerWarm = false;
    private bool isOutsideWarm = false;
    private bool isStressed = false;

    public event Action<float> ShowHP;
    public event Action<bool> ChangeAnimation;
    public event Action GameOver;
    public event Action ChangeToGameOverAnimation;

    public void ChangeDamageState(bool? isHouseWarm)
    {
        //Debug.Log(isPlayerWarm == isHouseWarm);
        isStressed = isPlayerWarm == isHouseWarm;
    } 

    public void CheckOutsideAir()
    {
        isStressed = isPlayerWarm == isOutsideWarm;
    }

    public void ChangeOutSideAirState(bool b)
    {
        isOutsideWarm = b;
        if (GameManagerModel.currentState != GameManagerModel.GameState.notStarted) return;
        ChangePlayerState(!isOutsideWarm);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;
        if (isStressed)
        {
            Debug.Log("isssss");
            playerHP -= damageSpeed * Time.deltaTime;
            CancelInvoke(nameof(HealDamage));
            if (playerHP <= 0)
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
    }

    void HealDamage()
    {
        playerHP += healingSpeed * Time.deltaTime;
        if (playerHP > 1) playerHP = 1;
    }

    public void ChangePlayerState(bool isWarm)
    {
        isPlayerWarm = isWarm;
        ChangeAnimation?.Invoke(isPlayerWarm);
    }

    public bool ReturnIsStressed()
    {
       // Debug.Log(isStressed);
        return isStressed;
    }
}
