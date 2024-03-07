using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindWeigth : MonoBehaviour
{
    void Update()
    {
        Vector3 scale;

        WindPoint.GetWeightAt(transform.position, out scale, out Vector3 forward);

        transform.localScale = scale;
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
