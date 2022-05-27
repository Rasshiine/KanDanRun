using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectCreatorModel : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Transform[] bg01Pos;
    [SerializeField] private Transform[] bg02Pos;
    private SpriteRenderer[] bg02Image;
    [SerializeField] private Transform[] groundPos;

    [SerializeField] private GameObject[] clouds;

    private Vector3 defaultPos = new Vector3(10, 0, 0.1f);
    private Vector3 moveDistanceVector;
    private Vector3 cloudMovingVector = new Vector3(-0.2f, 0, 0);
    private float bg01Magnification = 0.2f;
    private float bg02Magnification = 0.4f;

    void Awake()
    {
        bars = new GameObject[barCount];
        barDeadline = defaultBarPos.x - distance * objectileCount * 2;
        defaultBg02Pos = new Vector3(30, bg02Pos[0].position.y, 2);
        bg02Image = new SpriteRenderer[2];
        for (int i = 0; i < 2; i++)
            bg02Image[i] = bg02Pos[i].gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Create();
        HouseBeatAnimation();
        ChangeBGColor(0);
    }

    private Tween[] houseBeatTween = new Tween[2];
    float pitch = 1;
    void HouseBeatAnimation()
    {
        for(int i = 0; i < 2; i++)
        {
            houseBeatTween[i] = bg02Pos[i].DOScaleY(0.3f, GameManagerView.DefaultPitch)
            .SetRelative(true)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo);
        }
    }


    int currentBarNum = 0;
    int currentbg02Num = 0;
    int barCount = 10;
    int objectileCount = 10;
    float distance = 10;

    float barDeadline;
    private Vector3 defaultBarPos = new Vector3(25, 0, 1);

    float bg01deadLine = -20;
    //下の二つは微妙に位置調整した実質マジックナンバー
    float bg02deadLine = -30;
    private Vector3 defaultBg01Pos;
    private Vector3 defaultBg02Pos;
    private Vector3 defaultgroundPos;
    

    GameObject[] bars;

    float possibilityOfBoyCloud = 0.05f;

    void Create()
    {

        for(int i = 0; i < barCount; i++)
        {
            bars[i]=
            Instantiate(
                  new GameObject("bar" + i.ToString("D2")),
                  defaultBarPos,
                  Quaternion.identity);
            for (int j = 0; j < objectileCount; j++)
            {
                int num = UnityEngine.Random.Range(0, TutorialManager.isTutorialMode ? 1 : objects.Length);
                GameObject g = objects[num];
                Instantiate(
                    g,
                    defaultBarPos + Vector3.right * (distance * j + UnityEngine.Random.Range(-2f, 2f))
                    + g.transform.position,
                    Quaternion.identity)
                    .transform.parent
              = bars[i].transform;
            }
        }
        //bar一本分右に移動
        bars[1].transform.position += Vector3.right * distance * objectileCount;



        defaultgroundPos = groundPos[1].position;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.isTutorialMode && !TutorialManager.canBackGroundMove) return;
        //雲の移動
        for (int i = 0; i < bg01Pos.Length; i++)
        {
            bg01Pos[i].position += cloudMovingVector * Time.deltaTime;
            if (bg01Pos[i].position.x < bg01deadLine)
            {
                Destroy(bg01Pos[i].gameObject);
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                int tmp = 2;
                if (rand < (1 - possibilityOfBoyCloud) / 2) tmp = 0;
                else if(rand < (1 - possibilityOfBoyCloud)) tmp = 1;
                GameObject g = Instantiate(clouds[tmp], Vector3.zero, Quaternion.identity);
                bg01Pos[i] = g.transform;
                    
                bg01Pos[i].position = new Vector3(-bg01deadLine, UnityEngine.Random.Range(1f, 2f), 2);
            }
        }
        for (int i = 0; i < 2; i++) 
        houseBeatTween[i].timeScale = BGMManagerModel.bGMpitch;

        for (int i = 0; i < 2; i++)
        {
            //街並みを動かす
            if (GameManagerModel.currentState != GameManagerModel.GameState.Finished)
            {
                bg02Pos[i].position += moveDistanceVector * bg02Magnification * Time.deltaTime;
                groundPos[i].position += moveDistanceVector * Time.deltaTime;
            }

            //家を動かす
            if (GameManagerModel.currentState ==　GameManagerModel.GameState.Playing)
            bars[(currentBarNum + i) % barCount].transform.position += moveDistanceVector * Time.deltaTime;

            if (groundPos[i].position.x < -defaultgroundPos.x)
                groundPos[i].position = defaultgroundPos;
        }

        if (bg02Pos[currentbg02Num % 2].position.x < bg02deadLine)
        {
            bg02Pos[currentbg02Num % 2].position = defaultBg02Pos;
            currentbg02Num++;
        }

        

        if (GameManagerModel.currentState != GameManagerModel.GameState.Playing) return;        

        //家を初期位置に戻す
        if (bars[currentBarNum % barCount].transform.position.x < barDeadline)
        {
            bars[currentBarNum % barCount].transform.position = defaultBarPos;
            currentBarNum++;
        }
    }

    public void ChangeSpeed(float speed)
    {
        moveDistanceVector = new Vector3(-speed, 0, 0);
    }

    public void ChangeBGColor(float time)
    {
        Color c = GameManagerModel.isOutsideWarm ? Color.gray : Color.white;
        for (int i = 0; i < 2; i++)
        {
            bg02Image[i].DOColor(c, time);
        }

    }

    public void ChangeHouseBeatStatus(bool b)
    {
        for (int i = 0; i < 2; i++)
        {
            if (b) houseBeatTween[i].Play();
            else houseBeatTween[i].Pause();
        }
            
    }
}

