using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAddBuff : IBuff
{
    BuffData_SO buffData;

    public float originalRotateAddSpeed;
    public float buffRotateAddSpeed;
    public float duration;
    Coroutine rotateAddBuffCoroutine;

    public RotateAddBuff()
    {
        InitBuffData();
    }

    public void InitBuffData()
    {
        if(buffData == null)
        {
            buffData = Resources.Load<BuffData_SO>("RotateAddBuff_Data");
        }

        buffRotateAddSpeed = buffData.increaseValue;
        duration = buffData.duration;
    }

    public void ApplyBuff()
    {
        if(rotateAddBuffCoroutine != null) { CoroutineManager.Instance.StopCoroutine(rotateAddBuffCoroutine); }
        originalRotateAddSpeed = GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed;
        rotateAddBuffCoroutine = CoroutineManager.Instance.StartCoroutine(RotateAddBuffCoroutine());
    }

    IEnumerator RotateAddBuffCoroutine()
    {
        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().RotateAdd += buffRotateAddSpeed;
        Debug.Log("旋转Buff生效中......");

        yield return new WaitForSeconds(duration);

        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().RotateAdd = originalRotateAddSpeed;
        rotateAddBuffCoroutine = null;
        Debug.Log("旋转Buff消失......");
        yield return null;
    }
}
