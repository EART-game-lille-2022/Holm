using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindAeraParticle : MonoBehaviour
{
    private WindArea _windArea;
    private ParticleSystem _particle;
    #if UNITY_EDITOR
    void Update()
    {
        if(!Application.isPlaying)
            Start();
    }
    #endif
    void Start()
    {
        if (!_particle)
            _particle = GetComponent<ParticleSystem>();
        if(!_windArea)
            _windArea = transform.parent.GetComponent<WindArea>();

        ParticleSystem.ShapeModule shape = _particle.shape;
        BoxCollider collider = transform.parent.GetComponent<BoxCollider>();
        shape.scale = new Vector3(collider.size.x, collider.size.z, 1);
        transform.localPosition = new Vector3(0, -collider.size.y / 2, 0);
        
        var main = _particle.main;
        main.startLifetime = collider.size.y / main.startSpeedMultiplier;
        

        // main.startSize = transform.parent.localScale.magnitude;
        // float speed = transform.parent.localScale.y / _particle.main.startLifetime.constant;
        // main.startSpeed = speed;
    }
}
