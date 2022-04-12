using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPresenter : MonoBehaviour
{
    [SerializeField] GameManagerModel gameManagerModel;
    [SerializeField] GameManagerView gameManagerView;
    [SerializeField] ObjectCreatorModel objectCreatorModel;

    private void Awake()
    {
        //Model → View
        gameManagerModel.ShowScore += gameManagerView.ShowScore;
        gameManagerModel.ActivateUIs += gameManagerView.ActivateUIs;
        gameManagerModel.ChangeWeather += gameManagerView.ChangeWeather;
        gameManagerModel.ChangeWeatherWithNoMotion += gameManagerView.ChangeWeatherWithNoMotion;

        //View → Model
        gameManagerView.StartScene += gameManagerModel.StartScene;
        gameManagerView.GetScore += gameManagerModel.GetScore;

        gameManagerModel.ChangeSpeed += objectCreatorModel.ChangeSpeed;
    }
}
