using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlesYaya : MonoBehaviour
{
    public float speed;
    public Vector3 startpos;
    public float yOffSet;
    public float xOffSet;
    public float zOffSet;
    float randomness;

    void Start()
    {
        startpos = transform.position;
        // randomness = Random.value;
    }

    void Update()
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = startpos.x + (xOffSet * Mathf.Sin(Time.time * speed));
        newPos.y = startpos.y + (yOffSet * Mathf.Sin(Time.time * speed));
        newPos.z = startpos.z + (zOffSet * Mathf.Sin(Time.time * speed));
        transform.position = newPos;
    }
}
