using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoatController : MonoBehaviour, IBoatController
{
    #region Unity Base Method;
    private void Start()
    {
        
    }
    #endregion

    /// <summary>
    /// �����ƶ��߼�����ʱ�ÿ�
    /// </summary>
    public virtual void DoAction()
    {
        Debug.Log("��������");
    }
}
