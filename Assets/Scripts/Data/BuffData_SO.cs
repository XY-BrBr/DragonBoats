using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Buff��ֵ
/// </summary>
[SerializeField]
[CreateAssetMenu(fileName = "NewBuffData", menuName = "GameData/BuffData")]
public class BuffData_SO : ScriptableObject
{
    //Buff����
    public string buffName;

    //Buff����
    public BuffType buffType;

    //Buff��ֵ����
    public float increaseValue;

    //����ʱ��
    public float duration;
}
