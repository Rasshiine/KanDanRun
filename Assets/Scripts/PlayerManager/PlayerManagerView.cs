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
    [SerializeField] private Animator smokeAnimator;

    public event Action CheckOutsideAir;
    public event Action<bool?> ChangeDamageState;
    public event Action<bool> ChangePlayerState;
    public event Action<bool, bool> ChangeExactly;
    //Func:Actionの戻り値ありバージョン
    public event Func<bool> GetIsStressed;
    public event Func<bool> GetIsPlayerWarm;

    public event Action SE_ChangeCloth;



    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;
    
        //ここMVPじゃないかもだけど許して
        if (!isInHouse)
        {
            CheckOutsideAir?.Invoke();
        }
        animator.SetBool(NameKeys.anim_isStressed, GetIsStressed());
    }

    public void ChangeCloth(bool inp)
    {
        if (GetIsPlayerWarm() == inp) return;
        ChangeExactly?.Invoke(true, inp);
        ChangePlayerState?.Invoke(inp);
        smokeAnimator.SetTrigger(inp ? NameKeys.anim_beWarmTrigger : NameKeys.anim_beColdTrigger);
        //animator.SetTrigger(inp ? NameKeys.anim_beWarmTrigger : NameKeys.anim_beColdTrigger) ;
        SE_ChangeCloth?.Invoke();
    }

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SendChangeExactly(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    { 
        isInHouse = true;
        string tag = collision.gameObject.tag;
        ChangeDamageState.Invoke(tag == NameKeys.warmTag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SendChangeExactly(collision);
        isInHouse = false;
        ChangeDamageState.Invoke(null);
    }
    #endregion
    void SendChangeExactly(Collider2D collision)
    {
        bool b = collision.gameObject.tag == NameKeys.warmTag;
        ChangeExactly?.Invoke(false, b);
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