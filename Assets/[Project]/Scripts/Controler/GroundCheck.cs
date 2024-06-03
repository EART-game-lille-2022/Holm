using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _jumpGroundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _timeToUnground;
    private float _timer;
    RaycastHit _checkHit;
    PlayerControler _playerControler;
    private bool _hasGrounded;
    private PlayerAnimation _animation;
    public bool _hisGrounded;

    void Start()
    {
        _playerControler = GetComponent<PlayerControler>();
        _animation = transform.parent.GetComponentInChildren<PlayerAnimation>();
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
        if (!_hasGrounded)
            return false;

        RaycastHit hit;
        Physics.BoxCast(transform.position
                , Vector3.one / 2
                , Vector3.down
                , out hit
                , Quaternion.identity
                , _jumpGroundCheckDistance
                , _groundLayer);
        // print(hit.collider);
        return hit.collider;
    }

    public float overlapRaduis = 1f;
    public float overlapPoin1 = 1;

    private bool Check()
    {
        //! Si le cast commence a la frontiere d'un collider c la merde, attention au transform.position set au noveau du sol
        // Physics.BoxCast(transform.position + Vector3.up
        //                 , Vector3.one / 2
        //                 , Vector3.down
        //                 , out _checkHit
        //                 , Quaternion.identity
        //                 , _groundCheckDistance
        //                 , _groundLayer);

        Collider[] overlapCollider = Physics.OverlapCapsule(transform.position
                                                          , transform.TransformPoint(Vector3.down * overlapPoin1)
                                                          , overlapRaduis);

        // print("Collider overlap : " + overlapCollider.Length);
        for (int i = 0; i < overlapCollider.Length; i++)
        {
            // print(overlapCollider[i].name + " : " + overlapCollider[i].gameObject.layer);
            if (overlapCollider[i].gameObject.layer == 10)
            {
                // print("Overlap ground ! ");
                _hisGrounded = overlapCollider[i];
                return overlapCollider[i];
            }
        }

        // _animation.SetJump(false);
        // print("Don't Overlap ground !");
        _hisGrounded = false;
        return false;

        // print(_checkHit.collider ? "Hit " + _checkHit.collider.name : "No Hit");
        // Debug.DrawRay(transform.position, Vector3.down * _groundCheckDistance, Color.red);
        // _hisGrounded = _checkHit.collider;
        // return _checkHit.collider;
    }

    void OnCollisionEnter(Collision other)
    {
        // print(other.gameObject.layer);
        if (other.collider.gameObject.layer == 10)
        {
            // print("Collide with : " + other.collider.name);

            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance, _groundLayer);
            if (hit.collider && hit.normal.y > /* Vector3.up */ .5f)
            {
                // print("Normal check pass with : " + other.collider.name); 
                _hasGrounded = true;
                _playerControler.ChangeState(PlayerState.Grounded);
            }

            _animation.SetJump(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1 * overlapRaduis);
        Gizmos.DrawSphere(transform.TransformPoint(Vector3.down * overlapPoin1), 1 * overlapRaduis);
    }
}
