using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngelUtils
{
    /// <summary>
    /// 欧拉角转Inspector Rotate数值
    /// </summary>
    /// <param name="tran">需要转换的Transform</param>
    /// <returns></returns>
    public static Vector3 EulerAngles2InspectorRotation_Ex(this Transform tran)
    {
        Vector3 up = tran.up;
        Vector3 eulerAngle = tran.eulerAngles;
        Vector3 resVector = eulerAngle;

        if (Vector3.Dot(up, Vector3.up) >= 0f)
        {
            if (eulerAngle.x >= 0f && eulerAngle.x <= 90f) { resVector.x = eulerAngle.x; }
            if (eulerAngle.x >= 270f && eulerAngle.x <= 360f) { resVector.x = eulerAngle.x - 360f; }
        }

        if (Vector3.Dot(up, Vector3.up) < 0f)
        {
            if (eulerAngle.x >= 0f && eulerAngle.x <= 90f) { resVector.x = 180 - eulerAngle.x; }
            if (eulerAngle.x >= 270f && eulerAngle.x <= 360f) { resVector.x = 180 - eulerAngle.x; }
        }

        if (eulerAngle.y > 180)
            resVector.y = eulerAngle.y - 360f;

        if (eulerAngle.z > 180)
            resVector.z = eulerAngle.z - 360f;

        return resVector;
    }
}
