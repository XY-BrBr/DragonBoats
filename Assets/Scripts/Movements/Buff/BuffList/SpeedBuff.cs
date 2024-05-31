using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour, IBuff
{
    public float originalAddSpeed;
    public float buffAddSpeed;
    public float duration;
    public Coroutine speedBuffCoroutine;

    public void ApplyBuff()
    {
        if (speedBuffCoroutine != null) { StopCoroutine(speedBuffCoroutine); }
        originalAddSpeed = GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed;
        StartCoroutine(SpeedBuffCoroutine(duration));
    }

    private IEnumerator SpeedBuffCoroutine(float duration)
    {
        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed += buffAddSpeed;
        Debug.Log("����Buff��Чing......");

        yield return new WaitForSeconds(duration);
        GameManager.Instance.Ship.GetComponent<DragonBoatMovement>().AddSpeed = originalAddSpeed;
        Debug.Log("����Buff��ʧ......");
        speedBuffCoroutine = null;
    }
}

