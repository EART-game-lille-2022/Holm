using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    [SerializeField] private bool _isPlayerFlying;
    private Rigidbody _rigidbody;
    private ThridPersonControler _thridPersonControler;
    private FlyControler _flyControler;
    private float timer;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _thridPersonControler = GetComponent<ThridPersonControler>();
        _flyControler = GetComponent<FlyControler>();
        ChangePlayerFlystate(false);
    }

    void Update()
    {
        FallingCheck();
    }

    void FallingCheck()
    {
        if(_rigidbody.velocity.y < 0)
            timer += Time.deltaTime;
        else
            timer = 0;
        
        if(timer >= 2f)
        {
            timer = 0;
            ChangePlayerFlystate(true);
        }
    }

    public void ChangePlayerFlystate(bool isPlayerFlying)
    {
        _isPlayerFlying = isPlayerFlying;
        if(_isPlayerFlying)
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
}
