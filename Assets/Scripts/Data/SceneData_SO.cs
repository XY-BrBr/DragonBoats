using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "NewSceneData", menuName = "GameData/SceneData")]
public class SceneData_SO : ScriptableObject
{
    public List<GameObject> CheckPoint;

    public float resistanceSpeed = 0.1f;

    public int roundCount = 2;
}
