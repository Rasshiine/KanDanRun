using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPresenter : MonoBehaviour
{
    [SerializeField] GameManagerModel gameManagerModel;
    [SerializeField] GameManagerView gameManagerView;
    [SerializeField] ObjectCreatorModel objectCreatorModel;
    [SerializeField] SEManagerModel sEManagerModel;
    [SerializeField] BGMManagerModel bGMManagerModel;

    private void Awake()
    {
        //Model → View
        gameManagerModel.ShowScore += gameManagerView.ShowScore;
        gameManagerModel.ActivateUIs += gameManagerView.ActivateUIs;
        gameManagerModel.ChangeWeatherWithNoMotion += gameManagerView.ChangeWeatherWithNoMotion;
        gameManagerModel.GameStart += gameManagerView.StartButton;
        gameManagerModel.StartBlinking += gameManagerView.StartBlinking;

        //View → Model
        gameManagerView.StartScene += gameManagerModel.StartScene;
        gameManagerView.GetScore += gameManagerModel.GetScore;
        gameManagerView._ChangeWeather += gameManagerModel._ChangeWeather;

        //Model → ObjectCreatorModel
        gameManagerModel.ChangeSpeed += objectCreatorModel.ChangeSpeed;
        gameManagerModel.ChangeBGColor += objectCreatorModel.ChangeBGColor;

        //Model → SEManagerModel
        gameManagerModel.SE_ChangeWeather += sEManagerModel.ChangeWeather;
        gameManagerModel.SE_LevelUp += sEManagerModel.LevelUp;
        gameManagerModel.SE_GameOver += sEManagerModel.GameOverSound;

        //View → SEManagerModel
        gameManagerView.SE_StartButton += sEManagerModel.StartButton;
        gameManagerView.SE_BackButton += sEManagerModel.BackButton;

        //Model → BGMManagerModel
        gameManagerModel.IncreasePitch += bGMManagerModel.IncreasePitch;

        //View → BGMManagerModel
        gameManagerView.GetPitch += bGMManagerModel.GetPitch;

        
    }
}
