using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public static void StartRoutine(IEnumerator routine)
    {
        Instance.StartCoroutine(routine);
    }

    public static void StopRoutine(IEnumerator routine)
    {
        Instance.StopCoroutine(routine);
    }

    public static void StopAllRoutines()
    {
        Instance.StopAllCoroutines();
    }
}
