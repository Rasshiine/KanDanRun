using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerPresenter : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;

    [SerializeField] private PlayerManagerView playerManagerView;
    [SerializeField] private ObjectCreatorModel objectCreatorModel;

    private void Awake()
    {
        //TutorialManager → view
        tutorialManager.ChangeAnimationSpeed += playerManagerView.ChangeAnimationSpeed;

        //TutorialManager → ObjectCreatorModel
        tutorialManager.ChangeHouseBeatStatus += objectCreatorModel.ChangeHouseBeatStatus;
    }
}
