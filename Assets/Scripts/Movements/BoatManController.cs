using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BoatManController : BoatController
{
    //�ƶ��˰�ť
    public Canvas ShiperParent;
    public Button ShiperTowards_Btn;
    public Button ShiperOrder_Btn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoAction()
    {
        base.DoAction();
        Debug.Log("���ַ�����");
    }
}
