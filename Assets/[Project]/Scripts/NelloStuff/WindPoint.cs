using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindPoint : MonoBehaviour
{
    public static List<WindPoint> points = new List<WindPoint>();
    public float value = 1;
    // Start is called before the first frame update
    void OnEnable()
    {
        points.Add(this);
    }

    // Update is called once per frame
    void OnDisable()
    {
        points.Remove(this);
    }

    public static void GetWeightAt(Vector3 position, out float value, out Vector3 scale, out Vector3 forward)
    {
        float currentWeight = 0;
        value= 0;
        scale= Vector3.zero;
        forward = Vector3.zero;
        foreach (var point in points)
        {
            float d = Mathf.Max(0.0001f,(position - point.transform.position).magnitude);
            //d = d * d;
            float weight = 1f / d;
            scale += point.transform.localScale * weight;
            value += point.value * weight;
            forward += point.transform.forward * weight;
            currentWeight += weight;
        }
        scale /= currentWeight; 
        value /= currentWeight;
        forward/= currentWeight;

        //Vector2.Angle(forward, Vector.up);
        //float a = Mathf.Atan2(forward.x, -forward.y) * Mathf.Rad2Deg;
        //Vector2 dirA = new Vector2(Mathf.Sin(a * Mathf.Deg2Rad), Mathf.Cos(a * Mathf.Deg2Rad));

        //forward.VectorToAngle(dirA);

        //List<GameObject> list = new List<GameObject>();

        //GameObject ra = list.GetRandom<GameObject>();
        //WindPoint rap = points.GetRandom<WindPoint>();

        //float w = 10;
        //float result = w.Remap01(100, 50).Abs().Pow(2);
        //float r = Mathf.Pow(Mathf.Abs(ExtendsMethods.Remap01(w, 100, 50)), 2);
    }
}
