using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDragonBoatsData", menuName = "GameData/DragonBoatsData")]
public class DragonBoatData_SO : ScriptableObject
{
    [Header("������������")]
    [Tooltip("����ٶ�")]
    public float maxSpeed;

    [Tooltip("����ٶȣ���Ϳ�ת���ٶȣ�")]
    public float minSpeed;

    [Tooltip("ת���ٶ�")]
    public float rotateSpeed;

    [Tooltip("����ҡ���ٶ�")]
    public float shakeSpeed;

    //TODO:���ܺ����������(ѡ������)
    [Header("��������")]
    [Tooltip("���ټ��ٶ�")]
    public float addSpeed;

    [Tooltip("���ټ��ٶ�")]
    public float slowSpeed;

    [Tooltip("ת����ٶ�")]
    public float rotateAdd;

    [Tooltip("������б�ظ��ٶ�")]
    public float returnShakeSpeed;

    [Tooltip("ҡ���ٶ�")]
    public float shakeAdd;
}
