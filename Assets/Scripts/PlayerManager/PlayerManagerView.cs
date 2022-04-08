using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagerView : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;
    private Color32 red = new Color(255, 0, 0);
    private Color32 blue = new Color(0, 0, 255);

    [SerializeField] Image stressGage;

    private bool isInHouse = false;

    public event Action CheckOutsideAir;
    public event Action<bool?> ChangeDamageState;
    public event Action<bool> ChangePlayerState;

    private void Awake()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePlayerState?.Invoke(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePlayerState?.Invoke(true);
        }

        //ここMVPじゃないかもだけど許して
        if (!isInHouse)
        {
            CheckOutsideAir?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isInHouse = true;
        string tag = collision.gameObject.tag;
        if (tag == NameKeys.invincibleTag)
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

    public void ChangeColor(Color32 c)
    {
        playerSpriteRenderer.color = c;
    }

}
