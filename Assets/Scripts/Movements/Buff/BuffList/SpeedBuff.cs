using UnityEngine;
using System.Collections;

public class SpeedBuff : IBuff
{
    public BuffData_SO buffData;

    public float originalAddSpeed;
    public float buffAddSpeed = 0.1f;
    public float duration = 5;
    Coroutine speedBuffCoroutine;

    public SpeedBuff()
    {
        InitBuffData();
    }

    public void InitBuffData()
    {
        if(buffData == null)
        {
            buffData = Resources.Load<BuffData_SO>("SpeedBuff_Data");
        }

        buffAddSpeed = buffData.increaseValue;
        duration = buffData.duration;
        Debug.Log(buffAddSpeed+ "," +duration);
    }

    public void ApplyBuff()
    {
        if (speedBuffCoroutine != null) { CoroutineManager.Instance.StopCoroutine(speedBuffCoroutine); }
        originalAddSpeed = GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed;
        speedBuffCoroutine = CoroutineManager.Instance.StartCoroutine(SpeedBuffCoroutine());
    }

    IEnumerator SpeedBuffCoroutine()
    {
        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed += buffAddSpeed;
        Debug.Log("加速Buff生效ing......");
        //Debug.Log(GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed);

        yield return new WaitForSeconds(duration);

        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed = originalAddSpeed;
        Debug.Log("加速Buff消失......");
        speedBuffCoroutine = null;
        yield return null;
    }
}

