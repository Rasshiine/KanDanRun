using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreatorModel : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Transform[] bg01Pos;
    [SerializeField] private Transform[] bg02Pos;
    //private float[] lengths;
    private Vector3 defaultPos = new Vector3(10, 0, 0.1f);
    private Vector3 moveDistanceVector;

    private float bg01Magnification = 0.2f;
    private float bg02Magnification = 0.4f;

    void Awake()
    {
        bars = new GameObject[barCount];
        barDeadline = defaultBarPos.x - distance * objectileCount * 2;
        defaultBg02Pos = new Vector3(30, bg02Pos[0].position.y, 2);
    }

    void Start()
    {
        Create();
    }

    int currentBarNum = 0;
    int currentbg02Num = 0;
    int barCount = 10;
    int objectileCount = 10;
    float distance = 10;

    float barDeadline;
    private Vector3 defaultBarPos = new Vector3(25, -1, 1);

    float bg01deadLine = -10;
    //下の二つは微妙に位置調整した実質マジックナンバー
    float bg02deadLine = -30;
    private Vector3 defaultBg01Pos;
    private Vector3 defaultBg02Pos;

    GameObject[] bars;


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
                Instantiate(
                    objects[Random.Range(0, objects.Length)],
                    defaultBarPos + Vector3.right * distance * j,
                    Quaternion.identity)
                    .transform.parent
              = bars[i].transform;
            }
        }
        //bar一本分右に移動
        bars[1].transform.position += Vector3.right * distance * objectileCount;
    }
    

    // Update is called once per frame
    void Update()
    {
        //雲の移動
        for (int i = 0; i < bg01Pos.Length; i++)
        {
            bg01Pos[i].position += moveDistanceVector * 0.05f * Time.deltaTime;
            if (bg01Pos[i].position.x < bg01deadLine)
            {
                bg01Pos[i].position = new Vector3(30, Random.Range(1f, 2f), 2);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            //街並みを動かす
            if (GameManagerModel.currentState != GameManagerModel.GameState.Finished) 
            bg02Pos[i].position += moveDistanceVector * bg02Magnification * Time.deltaTime;

            //家を動かす
            if(GameManagerModel.currentState ==　GameManagerModel.GameState.Playing)
            bars[(currentBarNum + i) % barCount].transform.position += moveDistanceVector * Time.deltaTime;
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


}
