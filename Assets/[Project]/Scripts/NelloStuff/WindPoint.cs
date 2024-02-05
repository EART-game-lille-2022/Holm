using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[ExecuteInEditMode]
public class WindPoint : MonoBehaviour
{
    public static List<WindPoint> pointsList = new List<WindPoint>();
    public float value = 1;

    void OnEnable()
    {
        pointsList.Add(this);
    }

    void OnDisable()
    {
        pointsList.Remove(this);
    }

    public static void GetWeightAt(Vector3 position, out float value, out Vector3 scale, out Vector3 forward)
    {
        value = 0;
        scale = Vector3.zero;
        forward = Vector3.zero;
        float currentWeight = 0;

        foreach (var point in pointsList)
        {
            //! Max pour eviter la division par 0
            float distance = Mathf.Max(0.0001f, (position - point.transform.position).magnitude);
            // if(distance == 0)
            //     distance = .0001f;

            //! design patern pour get l'influence de plusieur points?
            float weight = 1f / distance;

            value += point.value * weight;
            scale += point.transform.localScale * weight;
            forward += point.transform.forward * weight;

            currentWeight += weight;
        }
        print("currentWeight : " + currentWeight);

        scale /= currentWeight;
        value /= currentWeight;
        forward /= currentWeight;

        //Vector2.Angle(forward, Vector.up);
        //float a = Mathf.Atan2(forward.x, -forward.y) * Mathf.Rad2Deg;
        //Vector2 dirA = new Vector2(Mathf.Sin(a * Mathf.Deg2Rad), Mathf.Cos(a * Mathf.Deg2Rad));

        //forward.VectorToAngle(dirA);

        //List<GameObject> list = new List<GameObject>();

        //GameObject ra = list.GetRandom<GameObject>();
        //WindPoint rap = points.GetRandom<WindPoint>();

        //float w = 10;
        // float result = w.Remap01(100, 50).Abs().Pow(2);
        // float r = Mathf.Pow(Mathf.Abs(ExtendsMethods.Remap01(w, 100, 50)), 2);
    }
}
