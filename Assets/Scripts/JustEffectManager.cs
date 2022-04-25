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

    [SerializeField] Text justText;
    [SerializeField] Image effectImage;
    [SerializeField] Sprite star;
    [SerializeField] Sprite flower;


    public event Action HealHP;

    private void Start()
    {
        justText.DOFade(0, 0);
        effectImage.DOFade(0, 0);
        defaultTextPosX = justText.rectTransform.anchoredPosition.x;
        defaultTextPosY = justText.rectTransform.anchoredPosition.y;
        defaultEffectPosX = effectImage.rectTransform.anchoredPosition.x;
        defaultEffectPosY = effectImage.rectTransform.anchoredPosition.y;
    }

    public void StartJustTimer()
    {
        JustEffect();
        Time.timeScale = slomoMagnification;
        DOVirtual.DelayedCall(0.1f, () => Time.timeScale = 1);
    }

    void JustEffect()
    {
        justText.DOFade(1, 0);
        DOTween.Sequence()
            .Join(justText.rectTransform.DOAnchorPos(new Vector2(defaultTextPosX, defaultTextPosY), 0))
            .Append(justText.rectTransform.DOAnchorPosY(10, 0.3f).SetRelative(true).SetEase(Ease.OutElastic))
            .Append(justText.DOFade(0, 0.3f))
            .SetUpdate(true);

        effectImage.sprite = GameManagerModel.isOutsideWarm ? flower : star;
        DOTween.Sequence()
        .Join(effectImage.rectTransform.DOAnchorPos(new Vector2(defaultEffectPosX, defaultEffectPosY), 0))
        .Join(effectImage.DOFade(1, 0))
        .Join(effectImage.rectTransform.DOAnchorPos(effectImageVector, 0.5f).SetRelative(true))
        .Join(effectImage.rectTransform.DORotate(new Vector3(0, 0, -90), 0.5f).SetRelative(true))

        .Join(effectImage.DOFade(0, 1f))
        .SetUpdate(true);
    }
}
