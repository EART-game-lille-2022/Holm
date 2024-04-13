using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindAeraParticle : MonoBehaviour
{
    private WindArea _windArea;
    private ParticleSystem _particle;

    void Update()
    {
        if (!_particle)
            _particle = GetComponent<ParticleSystem>();
        if(!_windArea)
            _windArea = transform.parent.GetComponent<WindArea>();

        ParticleSystem.ShapeModule shape = _particle.shape;
        shape.scale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.z, 1);
        transform.localPosition = new Vector3(0, -transform.parent.localScale.y / (2 * transform.parent.localScale.magnitude), 0);
        
        var main = _particle.main;

        // main.startSize = transform.parent.localScale.magnitude;
        float speed = transform.parent.localScale.y / _particle.main.startLifetime.constant;
        main.startSpeed = speed;
    }
}
