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
    /// 基本移动逻辑，暂时置空
    /// </summary>
    public virtual void DoAction()
    {
        Debug.Log("基本方法");
    }
}
