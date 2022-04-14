using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerPresenter : MonoBehaviour
{
    [SerializeField] PlayerManagerModel playerManagerModel;
    [SerializeField] PlayerManagerView playerManagerView;
    [SerializeField] GameManagerModel gameManagerModel;
    [SerializeField] SEManagerModel sEManagerModel;

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
        playerManagerView.ReturnIsStressed += playerManagerModel.ReturnIsStressed;

        //Model → GameManagerModel
        playerManagerModel.GameOver += gameManagerModel.GameOver;

        //GameManagerModel → Model
        gameManagerModel.ChangeOutSideAirState += playerManagerModel.ChangeOutSideAirState;

        //View → SEManagerModel
        playerManagerView.SE_ChangeCloth += sEManagerModel.ChangeCloth;
    }

}
