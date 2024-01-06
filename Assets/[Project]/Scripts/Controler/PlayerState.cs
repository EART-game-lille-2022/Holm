using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    [SerializeField] private bool _isPlayerFlying;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance;
    private Rigidbody _rigidbody;
    private ThridPersonControler _thridPersonControler;
    private FlyControler _flyControler;
    private float timer;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _thridPersonControler = GetComponent<ThridPersonControler>();
        _flyControler = GetComponent<FlyControler>();
        SetPlayerFlystate(false, true);
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    RaycastHit _hit;
    void GroundCheck()
    {
        //! Si le cast commence a la frontiere d'un collider c la merde, attention au transform.position set au noveau du sol
        Physics.BoxCast(transform.position + Vector3.up
                        , Vector3.one / 2
                        , Vector3.down
                        , out _hit
                        , Quaternion.identity
                        , _groundCheckDistance
                        , _groundLayer);

        Debug.DrawRay(transform.position, Vector3.down * _groundCheckDistance, Color.red);

        if (_hit.collider)
        {
            // print("Ground : " + _hit.collider.name);
            SetPlayerFlystate(false);
        }

        if (!_hit.collider)
        {
            SetPlayerFlystate(true);
        }
    }

    // void FallingCheck()
    // {
    //     if(_rigidbody.velocity.y < 0)
    //         timer += Time.deltaTime;
    //     else
    //         timer = 0;

    //     if(timer >= 2f)
    //     {
    //         timer = 0;
    //         SetPlayerFlystate(true);
    //     }
    // }

    public void SetPlayerFlystate(bool isPlayerFlying, bool skipGuard = false)
    {
        if (_isPlayerFlying == isPlayerFlying && !skipGuard)
            return;

        _isPlayerFlying = isPlayerFlying;
        if (_isPlayerFlying)
        {
            // print("Im flying !!!");
            _thridPersonControler.Disable();
            _flyControler.Activate();
        }
        else
        {
            // print("Im not flying !!!");
            _thridPersonControler.Activate();
            _flyControler.Disable();
        }
    }

    void OnDrawGizmos()
    {
        if (_hit.collider)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_hit.point, .3f);
        }
    }
}
