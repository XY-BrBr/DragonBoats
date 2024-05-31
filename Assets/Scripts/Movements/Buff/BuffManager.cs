using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BuffType
{
    Speed = 0b00000,
    MaxSpeed = 0b01110,
}

public class BuffManager : MonoBehaviour
{
    Dictionary<int, IBuff> buffDic = new Dictionary<int, IBuff>();

    public int currentBuff;

    //录入Buff
    public void InitBuff()
    {
        buffDic.Add((int)BuffType.Speed, new SpeedBuff());
        //buffDic.Add(new List<byte>() { 1, 1, 1, 1 }, BuffType.Power);
        //buffDic.Add(new List<byte>() { 1, 0, 1, 1 }, BuffType.Defense);
        buffDic.Add((int)BuffType.MaxSpeed, new MaxSpeedBuff());
    }

    //检查Buff类型
    public void CheckBuff(int currentBuff)
    {
        if(buffDic == null)
        {
            InitBuff();
        }

        if(buffDic.TryGetValue(currentBuff, out IBuff buff))
        {
            ApplyBuff(buff);
            currentBuff = 0;
        }
        else
        {

        }
    }

    // 运行Buff
    public void ApplyBuff(IBuff buff)
    {
        buff.ApplyBuff();
    }
}
