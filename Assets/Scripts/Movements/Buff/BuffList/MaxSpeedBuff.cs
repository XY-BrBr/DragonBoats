using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeedBuff : MonoBehaviour, IBuff
{
    BuffData_SO buffData;

    public float originalMaxSpeed;
    public float buffMaxSpeed;
    public float duration;
    Coroutine maxSpeedBuffCoroutine;

    public MaxSpeedBuff()
    {
        InitBuffData();
    }

    public void InitBuffData()
    {
        if(buffData == null)
        {
            buffData = Resources.Load<BuffData_SO>("MaxSpeedBuff_Data");
        }

        buffMaxSpeed = buffData.increaseValue;
        duration = buffData.duration;
    }

    public void ApplyBuff()
    {
        if(maxSpeedBuffCoroutine != null) { CoroutineManager.Instance.StopCoroutine(maxSpeedBuffCoroutine); }
        originalMaxSpeed = GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().MaxSpeed;
        maxSpeedBuffCoroutine = CoroutineManager.Instance.StartCoroutine(MaxSpeedBuffCoroutine());
    }

    IEnumerator MaxSpeedBuffCoroutine()
    {
        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().MaxSpeed += buffMaxSpeed;
        //Debug.Log(GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().MaxSpeed + "...." + GameManager.Instance.boatData.maxSpeed);
        Debug.Log("速度最大值Buff生效中.....");

        yield return new WaitForSeconds(duration);

        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().MaxSpeed = originalMaxSpeed;
        Debug.Log("速度最大值Buff消失......");
        maxSpeedBuffCoroutine = null;
        yield return null;
    }
}
