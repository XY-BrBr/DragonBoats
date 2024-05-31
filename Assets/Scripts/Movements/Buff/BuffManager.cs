using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

enum BuffType
{
    [Description("��߼��ٶ�")]
    Speed = 0b00000,

    [Description("�������ٶ�")]
    MaxSpeed = 0b01110,
}

public class BuffManager : MonoBehaviour
{
    Dictionary<BuffType, IBuff> buffDic = new Dictionary<BuffType, IBuff>();

    //¼��Buff
    public void InitBuff()
    {
        buffDic.Add(BuffType.Speed, new SpeedBuff());
        //buffDic.Add(new List<byte>() { 1, 1, 1, 1 }, BuffType.Power);
        //buffDic.Add(new List<byte>() { 1, 0, 1, 1 }, BuffType.Defense);
        buffDic.Add(BuffType.MaxSpeed, new MaxSpeedBuff());
    }

    //���Buff����
    public void CheckBuff(int _currentBuff)
    {
        BuffType currentBuff = (BuffType)_currentBuff;

        if(buffDic == null)
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
