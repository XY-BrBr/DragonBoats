using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Buff数值
/// </summary>
[SerializeField]
[CreateAssetMenu(fileName = "NewBuffData", menuName = "GameData/BuffData")]
public class BuffData_SO : ScriptableObject
{
    //Buff名称
    public string buffName;

    //Buff类型
    public BuffType buffType;

    //Buff增值数额
    public float increaseValue;

    //持续时间
    public float duration;
}
