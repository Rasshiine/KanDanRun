using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JustEffectManager : MonoBehaviour
{
    private float slomoMagnification = 0.01f;

    [SerializeField] Text justText;

    public event Action HealHP;

    public void StartJustTimer()
    {
        Time.timeScale = slomoMagnification;
        DOVirtual.DelayedCall(0.1f, () => Time.timeScale = 1);
    }
}
