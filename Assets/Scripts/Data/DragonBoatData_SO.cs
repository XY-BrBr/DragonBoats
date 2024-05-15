using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDragonBoatsData", menuName = "GameData/DragonBoatsData")]
public class DragonBoatData_SO : ScriptableObject
{
    [Header("龙船基本属性")]
    [Tooltip("最大速度")]
    public float maxSpeed;

    [Tooltip("最低速度（最低可转向速度）")]
    public float minSpeed;

    [Tooltip("转向速度")]
    public float rotateSpeed;

    [Tooltip("龙船摇晃速度")]
    public float shakeSpeed;

    //TODO:可能后续会变成玩家(选手属性)
    [Header("附加属性")]
    [Tooltip("增速加速度")]
    public float addSpeed;

    [Tooltip("减速加速度")]
    public float slowSpeed;

    [Tooltip("转向加速度")]
    public float rotateAdd;

    [Tooltip("船体倾斜回复速度")]
    public float returnShakeSpeed;

    [Tooltip("摇晃速度")]
    public float shakeAdd;
}
