using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JustEffectManager : MonoBehaviour
{
    private float slomoMagnification = 0.01f;

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

    private Tween textTween;


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
        .Join(justText.gameObject.transform.DOScale(1.2f, effectTime * 2)
        //.Join(justText.gameObject.transform.DOMoveY(0.2f, effectTime * 2)
        .SetEase(Ease.InBack).SetRelative())

        .Append(justText.DOFade(0, effectTime))
        .SetUpdate(true)
        .Pause()
        .SetAutoKill(false);
    }

    private void Start()
    {
        justText.DOFade(0, 0);
        effectImage.DOFade(0, 0);
    }

    public void StartJustTimer()
    {
        if (TutorialManager.isTutorialMode
            && GameManagerModel.currentState == GameManagerModel.GameState.notStarted) return;

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
}
