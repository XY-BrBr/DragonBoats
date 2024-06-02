using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum BuffType
{
    [Description("��߼��ٶ�")]
    Speed = 0b10000,

    [Description("�������ٶ�")]
    MaxSpeed = 0b11110,

    [Description("�����ת�ٶ�")]
    RotateSpeed = 0b111111,
}

public class BuffManager : MonoBehaviour
{
    Dictionary<BuffType, IBuff> buffDic = new Dictionary<BuffType, IBuff>();

    private void Start()
    {
        InitBuff();
    }

    //¼��Buff
    public void InitBuff()
    {
        buffDic.Add(BuffType.Speed, new SpeedBuff());
        //buffDic.Add(new List<byte>() { 1, 1, 1, 1 }, BuffType.Power);
        //buffDic.Add(new List<byte>() { 1, 0, 1, 1 }, BuffType.Defense);
        buffDic.Add(BuffType.MaxSpeed, new MaxSpeedBuff());
        buffDic.Add(BuffType.RotateSpeed, new RotateAddBuff());
    }

    //���Buff����
    public void CheckBuff(int _currentBuff)
    {
        BuffType currentBuff = (BuffType)_currentBuff;
        //Debug.Log(currentBuff);
        //Debug.Log(buffDic.ContainsKey(currentBuff));

        if(buffDic == null || buffDic.Count == 0)
        {
            InitBuff();
        }

        if(buffDic.TryGetValue(currentBuff, out IBuff buff))
        {
            UIManager.Instance.ShowBuff(currentBuff.GetDscription());
            ApplyBuff(buff);
        }
        else
        {
            UIManager.Instance.ShowBuff("��Ч");
            Debug.Log("</color = 'red'> Buff�ֵ���û�в��ҵ���Buff </color>");
        }
    }

    // ����Buff
    public void ApplyBuff(IBuff buff)
    {
        buff.ApplyBuff();
    }
}
