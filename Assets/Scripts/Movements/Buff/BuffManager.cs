using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

enum BuffType
{
    [Description("提高加速度")]
    Speed = 0b00000,

    [Description("提高最高速度")]
    MaxSpeed = 0b01110,
}

public class BuffManager : MonoBehaviour
{
    Dictionary<BuffType, IBuff> buffDic = new Dictionary<BuffType, IBuff>();

    //录入Buff
    public void InitBuff()
    {
        buffDic.Add(BuffType.Speed, new SpeedBuff());
        //buffDic.Add(new List<byte>() { 1, 1, 1, 1 }, BuffType.Power);
        //buffDic.Add(new List<byte>() { 1, 0, 1, 1 }, BuffType.Defense);
        buffDic.Add(BuffType.MaxSpeed, new MaxSpeedBuff());
    }

    //检查Buff类型
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
            UIManager.Instance.ShowBuff("无效");
            Debug.Log("</color = 'red'> Buff字典中没有查找到该Buff </color>");
        }
    }

    // 运行Buff
    public void ApplyBuff(IBuff buff)
    {
        buff.ApplyBuff();
    }
}
