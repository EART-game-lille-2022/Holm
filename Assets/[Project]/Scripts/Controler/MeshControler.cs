using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class RigMember
{
    public Transform rigTarget;
    private Vector3 startLocalPosition;
    public Vector3 StartPoint { get { return startLocalPosition; } set { startLocalPosition = value; } }
}

public class MeshControler : MonoBehaviour
{
    [SerializeField] private Rig _rig;
    [Space]
    [SerializeField] private Transform _controlerTransform;
    [SerializeField] private Vector3 _meshPositionOffset;
    [SerializeField] List<RigMember> _rigArmsList;
    [SerializeField] List<RigMember> _rigLegsList;
    [Space]
    [SerializeField] float _globalAnimationStrenght = .3f;
    [SerializeField] float _gloabalAnimationSpeed = 15f;
    [Space]
    [SerializeField] float _legsAnimationStrenght = 1f;

    private Rigidbody _playerRigidbody;
    private PlayerControler _playerControler;
    private Vector2 _inputDirection;

    void Start()
    {
        _playerRigidbody = transform.parent.GetComponentInChildren<Rigidbody>();
        _playerControler = transform.parent.GetComponentInChildren<PlayerControler>();

        foreach (var item in _rigArmsList)
            item.StartPoint = item.rigTarget.localPosition;
        foreach (var item in _rigLegsList)
            item.StartPoint = item.rigTarget.localPosition;
    }

    //! min = 25, / max = 55
    void Update()
    {
        FollowControler();
        SetRigWeight();

        _inputDirection = _playerControler.GetPlayerInput();
        foreach (var item in _rigArmsList)
            ArmsAnimation(item);

        foreach (var item in _rigLegsList)
            LegsAnimation(item);
    }

    private void SetRigWeight()
    {
        _rig.weight = _playerControler.GetPlayerState() == PlayerState.Flying ? 1 : 0;
    }

    private void ArmsAnimation(RigMember rigMember)
    {
        Vector3 newPos = Vector3.zero;
        float rigSide = Mathf.Sign(rigMember.StartPoint.x);
        float velocityVector = Mathf.Lerp(.5f, -5, Mathf.InverseLerp(25, 55, _playerRigidbody.velocity.magnitude));


        if (rigSide == 1)
            newPos.x = Mathf.Clamp(velocityVector * Mathf.Sign(rigMember.StartPoint.x), 0, 1);
        if (rigSide == -1)
            newPos.x = Mathf.Clamp(velocityVector * Mathf.Sign(rigMember.StartPoint.x), -1, 0);

        newPos.y = velocityVector;
        newPos.z = _inputDirection.x * rigSide;


        newPos *= _globalAnimationStrenght;
        rigMember.rigTarget.localPosition =
        Vector3.Lerp(rigMember.rigTarget.localPosition, newPos + rigMember.StartPoint, Time.deltaTime * _gloabalAnimationSpeed);
    }

    private void LegsAnimation(RigMember rigMember)
    {
        Vector3 newPos = Vector3.zero;
        float rigSide = Mathf.Sign(rigMember.StartPoint.x);
        float velocityVector = Mathf.Lerp(1f, -1, Mathf.InverseLerp(25, 55, _playerRigidbody.velocity.magnitude));

        if (rigSide == 1)
            newPos.x = Mathf.Clamp(velocityVector * Mathf.Sign(rigMember.StartPoint.x), 0, 1);
        if (rigSide == -1)
            newPos.x = Mathf.Clamp(velocityVector * Mathf.Sign(rigMember.StartPoint.x), -1, 0);

        newPos.y = velocityVector;
        newPos.z = _inputDirection.x * rigSide;

        newPos *= _globalAnimationStrenght * _legsAnimationStrenght;
        rigMember.rigTarget.localPosition =
        Vector3.Lerp(rigMember.rigTarget.localPosition, newPos + rigMember.StartPoint, Time.deltaTime * _gloabalAnimationSpeed);
    }

    private void FollowControler()
    {
        //TODO ajustter l'offset en fonction du stat du joueur
        transform.localPosition = _controlerTransform.localPosition + _meshPositionOffset;
        transform.localRotation = _controlerTransform.localRotation;
    }

    void OnValidate()
    {
        FollowControler();
    }

    public void SetRigValue(float value)
    {
        _rig.weight = value;
    }
}
