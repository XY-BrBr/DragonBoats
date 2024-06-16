using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "NewRewardData", menuName = "GameData/RewardData")]
public class RewardData_SO : ScriptableObject
{
    public int GoldCount;

    public int SilverCount;

    public int CopperCount;
}
