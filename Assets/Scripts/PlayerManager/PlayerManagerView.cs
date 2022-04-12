using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagerView : MonoBehaviour
{
    [SerializeField] Image stressGage;

    private bool isInHouse = false;

    private Animator animator;

    public event Action CheckOutsideAir;
    public event Action<bool?> ChangeDamageState;
    public event Action<bool> ChangePlayerState;
    //Func:Actionの戻り値ありバージョン
    public event Func<bool> ReturnIsStressed;



    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(animator);
            ChangeAnimation(false);
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePlayerState?.Invoke(false);
            animator.SetBool(NameKeys.anim_isPlayerWarm, false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePlayerState?.Invoke(true);
            animator.SetBool(NameKeys.anim_isPlayerWarm, true);
        }

        //ここMVPじゃないかもだけど許して
        if (!isInHouse)
        {
            CheckOutsideAir?.Invoke();
        }
        animator.SetBool(NameKeys.anim_isStressed, ReturnIsStressed());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isInHouse = true;
        string tag = collision.gameObject.tag;
        if (tag == NameKeys.invincibleTag && !ReturnIsStressed())
        {
            ChangeDamageState.Invoke(null);
            return;
        }
        ChangeDamageState.Invoke(tag == NameKeys.warmTag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInHouse = false;
        ChangeDamageState.Invoke(null);
    }

    public void ShowPlayerHP(float hp)
    {
        stressGage.fillAmount = hp;
    }

    //↓AnimationのStateとか、全部boolじゃなくてenumでやった方が良さそう
    public void ChangeAnimation(bool isPlayerWarm)
    {
        animator.SetBool(NameKeys.anim_isPlayerWarm, isPlayerWarm);
    }

    public void ChangeToGameOverAnimation()
    {
        animator.SetBool(NameKeys.anim_isGameOver, true);
    }
}