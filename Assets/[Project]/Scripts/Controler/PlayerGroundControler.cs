using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundControler : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _moveAxis;

    void Update()
    {
        transform.Translate(_moveAxis * _speed * Time.deltaTime);
    }

    public void SetMoveAxis(Vector3 value)
    {
        _moveAxis = value;
    }
}
