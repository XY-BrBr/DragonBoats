using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  AnimationUtils
{
    public static void AnimaSetBool(this List<Animator> anim, string name, bool set)
    {
        foreach (var a in anim)
        {
            a.SetBool(name, set);
        }
    }

    public static void AnimaSetTrigger(this List<Animator> anim, string name)
    {
        foreach (var a in anim)
        {
            a.SetTrigger(name);
        }
    }
}
