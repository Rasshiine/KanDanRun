using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Fungus;

public class TutorialManager : MonoBehaviour
{
    public static bool isTutorialMode;
    [SerializeField] Flowchart flowchart;

    [SerializeField] GameObject coldHouse;
    [SerializeField] Image blackImage;

    public static int progress = 0;

    private Vector3 moveVector = new Vector3(-5, 0, 0);

    private bool canColdHouseMove = false;
    public static bool canBackGroundMove = true;

    public event Action<float> ChangeAnimationSpeed;
    public event Action<bool> ChangeHouseBeatStatus;

    private void Awake()
    {
        progress = 0;
        isTutorialMode = SceneManager.GetActiveScene().name == NameKeys.tutorialScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isTutorialMode) return;
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTutorialMode) return;
        if (canColdHouseMove) MoveColdHouse();
    }

    //1:flowchart
    //2:PlayerManagerView
    //3:PlayerManagerView
    //4:PlayerManagerView
    //5:PlayerManagerView
    //6:


    IEnumerator Tutorial()
    {
        //flowchart.SendFungusMessage("EnteredColdHouse");

        while (progress == 0) yield return null;

        canColdHouseMove = true;
        while (progress == 1) yield return null;

        ChangeTimeStatus(false);
        blackImage.DOFade(0.25f, 0.2f);
        flowchart.SendFungusMessage("EnteredColdHouse");

        //PlayerManagerView(2)
        while (progress == 2) yield return null;

        ChangeTimeStatus(true);
        blackImage.DOFade(0, 0.2f);
        
        flowchart.SendFungusMessage("CloseChanged1");
        while (progress == 3) yield return null;

        ChangeTimeStatus(false);
        while (progress == 4) yield return null;

        while (progress == 5) yield return null;

        ChangeTimeStatus(true);
        flowchart.SendFungusMessage("CloseChanged2");

        while (progress == 6) yield return null;

        ChangeTimeStatus(false);
        while (progress == 7) yield return null;

        ChangeTimeStatus(true);
        flowchart.SendFungusMessage("CloseChanged3");
        while (progress == 8) yield return null;

        GameManagerModel.currentState = GameManagerModel.GameState.Playing;
        while (progress == 9) yield return null;
        flowchart.SendFungusMessage("CloseChanged4");
    }

    public static void SetProgress(int v) {
        if (progress >= v || progress + 1 != v) return;
            progress = v;
    }

    void MoveColdHouse()
    {

        coldHouse.transform.position += moveVector * Time.deltaTime;
    }

    void ChangeTimeStatus(bool status)
    {
        canBackGroundMove = status;
        canColdHouseMove = status;
        ChangeAnimationSpeed?.Invoke(status ? 1 : 0);
        ChangeHouseBeatStatus?.Invoke(status);
    }


    public void  SetProgress_(int v)
    {
        if (progress >= v || progress + 1 != v) return;
        progress = v;
    }

   
}
