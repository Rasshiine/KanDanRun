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


    private Color32 red = new Color(255, 0, 0);
    private Color32 blue = new Color(0, 0, 255);

    //private enum PlayerState
    //{
    //    Warm = 0,
    //    Cool = 1
    //}


    //private PlayerState state;
    //private PlayerState outsideAir;

    public event Action<float> ShowHP;
    public event Action<Color32> ChangeColor;
    public event Action GameOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeDamageState(bool? b)
    {
        isStressed = isPlayerWarm == b;
    } 

    public void CheckOutsideAir()
    {
        isStressed = isPlayerWarm == isOutsideWarm;
    }

    public void ChangeOutSideAirState(bool b)
    {
        isOutsideWarm = b;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isStressed)
        {
            playerHP -= damageSpeed * Time.deltaTime;
            CancelInvoke(nameof(HealDamage));
            if (playerHP <= 0)
            {
                GameOver?.Invoke();
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

    public void LetPlayerCool()
    {
        isPlayerWarm = false;
        ChangeColor?.Invoke(blue);
    }

    public void LetPlayerWarm()
    {
        isPlayerWarm = true;
        ChangeColor?.Invoke(red);
    }

}
