using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerPresenter : MonoBehaviour
{
    [SerializeField] private PlayerManagerModel playerManagerModel;
    [SerializeField] private PlayerManagerView playerManagerView;
    [SerializeField] private GameManagerModel gameManagerModel;
    [SerializeField] private SEManagerModel sEManagerModel;
    [SerializeField] private MobileInputModel mobileInputModel;
    [SerializeField] private JustEffectManager justEffectManager;

    private void Awake()
    {
        //Model → View
        playerManagerModel.ShowHP += playerManagerView.ShowPlayerHP;
        playerManagerModel.ChangeAnimation += playerManagerView.ChangeAnimation;
        playerManagerModel.ChangeToGameOverAnimation += playerManagerView.ChangeToGameOverAnimation;
        
        //View → Model
        playerManagerView.CheckOutsideAir += playerManagerModel.CheckOutsideAir;
        playerManagerView.ChangeDamageState += playerManagerModel.ChangeDamageState;
        playerManagerView.ChangePlayerState += playerManagerModel.ChangePlayerState;
        playerManagerView.GetIsStressed += playerManagerModel.GetIsStressed;
        playerManagerView.GetIsPlayerWarm += playerManagerModel.GetisPlayerWarm;
        playerManagerView.ChangeExactly += playerManagerModel.ChangeExactly;

        //Model → GameManagerModel
        playerManagerModel.GameOver += gameManagerModel.GameOver;

        //GameManagerModel → Model
        gameManagerModel.ChangeOutSideAirState += playerManagerModel.ChangeOutSideAirState;

        //Model → SEManagerModel
        playerManagerModel.PlayJustSound += sEManagerModel.Just;

        //View → SEManagerModel
        playerManagerView.SE_ChangeCloth += sEManagerModel.ChangeCloth;

        //MobileInputModel → View
        mobileInputModel.ChangeCloth += playerManagerView.ChangeCloth;

        //Model → justEffectManager
        playerManagerModel.StartJustTimer += justEffectManager.StartJustTimer;

    }

}
