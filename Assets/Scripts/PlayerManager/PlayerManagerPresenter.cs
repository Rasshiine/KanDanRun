using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerPresenter : MonoBehaviour
{
    [SerializeField] PlayerManagerModel playerManagerModel;
    [SerializeField] PlayerManagerView playerManagerView;
    [SerializeField] GameManagerModel gameManagerModel;

    private void Awake()
    {
        //Model → View
        playerManagerModel.ShowHP += playerManagerView.ShowPlayerHP;
        playerManagerModel.ChangeColor += playerManagerView.ChangeColor;

        //View → Model
        playerManagerView.CheckOutsideAir += playerManagerModel.CheckOutsideAir;
        playerManagerView.ChangeDamageState += playerManagerModel.ChangeDamageState;
        playerManagerView.LetPlayerCool += playerManagerModel.LetPlayerCool;
        playerManagerView.LetPlayerWarm += playerManagerModel.LetPlayerWarm;

        //Model → GameManagerModel
        playerManagerModel.GameOver += gameManagerModel.GameOver;

        //GameManagerModel → Model
        gameManagerModel.ChangeOutSideAirState += playerManagerModel.ChangeOutSideAirState;
    }

}
