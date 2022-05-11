using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MobileInputModel : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject face;

    private float startPosY;
    private float currentPosY;
    private float swipeDistance = 1.0f;

    public event Action<bool> ChangeCloth;


    // Update is called once per frame
    void Update()
    {
        if (!TutorialManager.isTutorialMode &&
            GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;

        if (TutorialManager.isTutorialMode &&
            (TutorialManager.progress != 2
            && TutorialManager.progress != 5
            && TutorialManager.progress != 7)
            && TutorialManager.progress != 9) return;

        if (Input.GetMouseButtonDown(0))
        {
            startPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        }

        if (Input.GetMouseButton(0))
        {
            face.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
            if (Mathf.Abs(startPosY - currentPosY) < swipeDistance) return;
            ChangeCloth(startPosY > currentPosY);
            startPosY = currentPosY;
        }

        if (Input.GetMouseButtonUp(0))
        {
            startPosY = 0;
            currentPosY = 0;
        }
        
        bool? inp = null;
        if (Input.GetKeyDown(KeyCode.UpArrow)) inp = false;
        if (Input.GetKeyDown(KeyCode.DownArrow)) inp = true;
        if (inp == null) return;
        ChangeCloth((bool)inp);
    }

}
