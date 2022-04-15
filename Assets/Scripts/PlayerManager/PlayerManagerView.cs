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
        animator.SetBool(NameKeys.anim_isStressed, ReturnIsStressed());

        bool? inp = null;
        if (Input.GetKeyDown(KeyCode.UpArrow)) inp = false;
        if (Input.GetKeyDown(KeyCode.DownArrow)) inp = true;
        if (inp == null) return;

        ChangePlayerState?.Invoke((bool)inp);
        animator.SetBool(NameKeys.anim_isPlayerWarm, (bool)inp);
        SE_ChangeCloth?.Invoke();
    }

    bool isStressed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag != NameKeys.House_FrontTag) return;
        isStressed = ReturnIsStressed();
    }
    bool a = true;
    private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (a != ReturnIsStressed())
    //    {
    //        Debug.Log("ReturnIsStressed= " +ReturnIsStressed());
    //        Debug.Log("Isinhouse = " + isInHouse);

    //        a = !a;
    //    }


    { 

        isInHouse = true;
        string tag = collision.gameObject.tag;
        //Debug.Log("ReturnIsStressed = " + ReturnIsStressed());

        //if (tag == NameKeys.invincibleTag && !ReturnIsStressed())
        //{
        //    ChangeDamageState.Invoke(null);
        //    return;
        //}

        if (tag == NameKeys.House_FrontTag && isStressed)
        {
            ChangeDamageState.Invoke(null);
            return;
        }
        //if (tag == NameKeys.invincibleTag && !ReturnIsStressed()) Debug.Break();
        ChangeDamageState.Invoke(tag == NameKeys.warmTag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != NameKeys.House_BackTag) return;
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