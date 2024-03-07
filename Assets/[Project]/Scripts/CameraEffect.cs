using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffect : MonoBehaviour
{
    [Header("FOV Parameter :")]
    [SerializeField] private float _minFovVelocity = 30;
    [SerializeField] private float _maxFovVelocity = 70;
    [SerializeField] private float _maxFov = 100;
    [SerializeField] private float _minFov = 70;

    [Header("Camera Shake :")]
    [SerializeField] private float _minShakeVelocity = 30;
    [SerializeField] private float _maxShakeVelocity = 70;
    [SerializeField] private float _maxShake = 2;
    [SerializeField] private float _minShake = 0;



    private CinemachineVirtualCamera _virtualCam;

    void Start()
    {
        _virtualCam = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void SetCameraFovWithVelocity(float velocityMag)
    {
        float newFov = Mathf.Lerp(_minFov, _maxFov, Mathf.InverseLerp(_minFovVelocity, _maxFovVelocity, velocityMag));
        _virtualCam.m_Lens.FieldOfView = newFov;
    }

    public void ShakeCameraWithVelocity(float velocityMag)
    {

    }
}
