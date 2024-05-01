using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WorldLimiter : MonoBehaviour
{
    [SerializeField] private Vector3 _min;
    [SerializeField] private Vector3 _max;

    void Update()
    {
        if (transform.position.x > _max.x || transform.position.y > _max.y || transform.position.z > _max.z)
            Reset();

        if (transform.position.x < _min.x || transform.position.y < _min.y || transform.position.z < _min.z)
            Reset();
    }

    private void Reset()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(_min.x, _max.x)
                                       , _max.y
                                       , Random.Range(_min.z, _max.z));

        GetComponent<Rigidbody>().MovePosition(spawnPoint * .9f);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
