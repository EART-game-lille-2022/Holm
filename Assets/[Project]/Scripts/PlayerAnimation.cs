using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string _xVectorName = "Xvector";
    [SerializeField] private string _yVectorName = "Yvector";
    [Space]
    [SerializeField] private string _groundName = "grounded";
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetRun(bool value)
    {
        _animator.SetBool("Run", value);
    }

    public void SetJump(bool value)
    {
        _animator.SetBool("Jump", value);
    }

    public void SetGround(bool value)
    {
        _animator.SetBool(_groundName, value);
    }

    public void SetXVector(float value)
    {
        _animator.SetFloat(_xVectorName, value);
    }

    public void SetYVector(float value)
    {
        _animator.SetFloat(_yVectorName, value);
    }
}
