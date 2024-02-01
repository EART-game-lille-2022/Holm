using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendsMethods
{
    public static float VectorToAngle(this Vector3 v1, Vector3 v2)
    {
        return Vector3.Angle(v1, v2);
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    public static float Remap01(this float f, float min, float max)
    {
        return (f - min) / (max - min);
    }
    public static float Remap(this float f, float minOld, float maxOld, float minNew, float maxNew)
    {
        return minNew + (f - minOld) / (maxOld - minOld) * (maxNew-minNew);
    }

    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
    }
    public static float Sign(this float f)
    {
        return Mathf.Sign(f);
    }
    public static float Pow(this float f, float p = 1)
    {
        return Mathf.Pow(f, p);
    }

    //public static GameObject GetRandom(this List<GameObject> list)
    //{
    //    return list[Random.Range(0, list.Count)];
    //}
}
