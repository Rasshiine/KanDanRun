using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreditManager : MonoBehaviour
{
    [SerializeField] private Transform bar;

    private float distanse = -95f;
    private float time = 20f;
    private float waitingTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        bar.DOMoveX(distanse, time)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(waitingTime, () =>
                {
                    SceneLoader.inst.LoadScene(NameKeys.mainScene);
                });
            });
    }
}
