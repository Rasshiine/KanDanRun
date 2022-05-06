using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class test : MonoBehaviour
{
    [SerializeField] Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    // Update is called once per frame
    bool b;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //b = !b;
            //flowchart.SetBooleanVariable("aaa", b);
            //Debug.Log("yobareta");
            flowchart.SendFungusMessage("XXX");
        }
    }
}
