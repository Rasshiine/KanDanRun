using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JustEffectManager : MonoBehaviour
{
    private float slomoMagnification = 0.01f;
    private float defaultEffectPosX;
    private float defaultEffectPosY;
    private float defaultTextPosX;
    private float defaultTextPosY;

    private Vector2 effectImageVector = new Vector2(20, 20);

    private Color32 noonTextColor = new Color32(224, 128, 205, 255);
    private Color32 nightTextColor = Color.white;

    [SerializeField] SpriteRenderer justText;
    [SerializeField] SpriteRenderer effectImage;
    [SerializeField] Sprite star;
    [SerializeField] Sprite flower;
    [SerializeField] GameObject justSmoke;
    [SerializeField] ParticleSystem justParticle;

    private Sequence justSeq;

    private float textMoveDist = 0.5f;
    private float effectTime = 0.2f;
    private Vector2 effectVec = new Vector2(0.5f, 0.5f);


    public event Action HealHP;

    private void Start()
    {
        justText.DOFade(0, 0);
        effectImage.DOFade(0, 0);
        //justText.gameObject.SetActive(false);
        //effectImage.gameObject.SetActive(false);
        defaultTextPosX = justText.gameObject.transform.position.x;
        defaultTextPosY = justText.gameObject.transform.position.y;
        //defaultEffectPosX = effectImage.rectTransform.anchoredPosition.x;
        //defaultEffectPosY = effectImage.rectTransform.anchoredPosition.y;
    }

    public void StartJustTimer()
    {
        PlayParticle();
        textTween.Restart();
        Time.timeScale = slomoMagnification;
        justSmoke.SetActive(true);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            justSmoke.SetActive(false);
            Time.timeScale = 1;
        });
    }

    void PlayParticle()
    {
        Sprite spr = GameManagerModel.isOutsideWarm ? flower : star;
        justParticle.textureSheetAnimation.SetSprite(0, spr);
        justParticle.Play();
    }


    Tween textTween;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayParticle();
            textTween.Restart();
        }
    }

    private void Awake()
    {
        textTween = DOTween.Sequence()

        //初期化
        .OnPlay(() =>
        {
            justText.color =
            GameManagerModel.isOutsideWarm ? noonTextColor : nightTextColor;
        })
        .Append(justText.DOFade(1, 0))

        //Position
        .Append(justText.gameObject.transform.DOMoveY(textMoveDist, effectTime)
        .SetEase(Ease.OutElastic).SetRelative())

        //Scale
        .Join(justText.gameObject.transform.DOScale(1.2f,effectTime*2)
        .SetEase(Ease.InBack).SetRelative())
        
        .Append(justText.DOFade(0, effectTime))
        .Pause()
        .SetAutoKill(false);
    }

    //void TweenTest()
    //{
    //    Debug.Log(justText.color.a);

    //    textTween = DOTween.Sequence()
    //    .Append(justText.DOFade(1, 0))
    //    .Append(justText.gameObject.transform.DOMoveY(textMoveDist, effectTime))

    //    .Append(justText.DOFade(0, effectTime))
    //    .OnUpdate(() => Debug.Log(justText.transform.position.y))
    //    //.SetAutoKill(false)
    //    .Pause();

    //        //.OnComplete(() =>
    //        //{
    //        //    justText.DORewind();
    //        //});
        
    //}

    void JustEffect()
    {
        //justText.DOFade(1, 0);
        //DOTween.Sequence()
        //    .Join(justText.rectTransform.DOAnchorPos(new Vector2(defaultTextPosX, defaultTextPosY), 0))
        //    .Append(justText.rectTransform.DOAnchorPosY(10, 0.3f).SetRelative(true).SetEase(Ease.OutElastic))
        //    .Append(justText.DOFade(0, 0.3f))
        //    .SetUpdate(true);

        //effectImage.sprite = GameManagerModel.isOutsideWarm ? flower : star;
        //DOTween.Sequence()
        //.Join(effectImage.rectTransform.DOAnchorPos(new Vector2(defaultEffectPosX, defaultEffectPosY), 0))
        //.Join(effectImage.DOFade(1, 0))
        //.Join(effectImage.rectTransform.DOAnchorPos(effectImageVector, 0.5f).SetRelative(true))
        //.Join(effectImage.rectTransform.DORotate(new Vector3(0, 0, -90), 0.5f).SetRelative(true))

        //.Join(effectImage.DOFade(0, 1f))
        //.SetUpdate(true);


        

        //if (justSeq != null)
        //{
        //    justSeq.Restart();
        //    return;
        //}

        effectImage.sprite = GameManagerModel.isOutsideWarm ? flower : star;

        justSeq = DOTween.Sequence()
            //初期化

            //.Append(justText.DOFade(1, 0))
            //.Append(effectImage.DOFade(1, 0))
            .OnPlay(() =>
            {
                //ustSeq.Restart();
                //effectImage.DOFade(0, 0);
                //justText.DOFade(0, 0);
            })


            .Join(justText.gameObject.transform.DOMoveY(textMoveDist, effectTime)
            .SetEase(Ease.OutElastic))
            .Append(effectImage.DOFade(1, effectTime))
            .Join(justText.DOFade(1, effectTime))
            
            .Join(effectImage.gameObject.transform.DOMove(effectVec, effectTime))
            .Join(effectImage.DOFade(-1f, effectTime))
            .Append(justText.DOFade(-1f, effectTime))

            .SetRelative(true)

            .SetUpdate(true)
            .OnUpdate(() => Debug.Log(effectImage.color.a))
            .OnComplete(() =>
            {
                //justSeq.Pause();
                justSeq.Rewind();
                //justText.DOFade(0, 0);
                //effectImage.DOFade(0, 0);
            });
        
    }
}
