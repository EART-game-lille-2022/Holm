using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _jumpGroundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _timeToUnground;
    // [SerializeField] private bool _isGrounded;
    private float _timer;
    RaycastHit _checkHit;
    PlayerControler _playerControler;
    private bool _hasGrounded;

    void Start()
    {
        _playerControler = GetComponent<PlayerControler>();
    }

    void FixedUpdate()
    {
        if (!Check())
        {
            _timer += Time.deltaTime;
            if (_timer > _timeToUnground)
            {
                _playerControler.ChangeState(PlayerState.Flying);
                _timer = 0;
                return;
            }
        }
    }

    public bool CanJump()
    {
        if(!_hasGrounded)
            return false;
        
        RaycastHit hit;
        Physics.BoxCast(transform.position
                , Vector3.one / 2
                , Vector3.down
                , out hit
                , Quaternion.identity
                , _jumpGroundCheckDistance
                , _groundLayer);
        print(hit.collider);
        return hit.collider;
    }

    private bool Check()
    {
        //! Si le cast commence a la frontiere d'un collider c la merde, attention au transform.position set au noveau du sol
        Physics.BoxCast(transform.position
                        , Vector3.one / 2
                        , Vector3.down
                        , out _checkHit
                        , Quaternion.identity
                        , _groundCheckDistance
                        , _groundLayer);

        // print(_checkHit.collider ? "Hit" : "No Hit");
        Debug.DrawRay(transform.position, Vector3.down * _groundCheckDistance, Color.red);
        return _checkHit.collider;
    }

    void OnCollisionEnter(Collision other)
    {
        // print(other.gameObject.layer);
        if (other.collider.gameObject.layer == 10)
        {
            // print(other.collider.gameObject.layer);

            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance, _groundLayer);
            if (hit.collider && hit.normal == Vector3.up)
            {
                _hasGrounded = true;
                _playerControler.ChangeState(PlayerState.Grounded);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_checkHit.collider)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_checkHit.point, .3f);
        }
    }
}
