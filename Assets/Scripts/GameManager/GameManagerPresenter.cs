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
        gameManagerModel.ChangeSpeed += objectCreatorModel.ChangeSpeed;

    }
}
