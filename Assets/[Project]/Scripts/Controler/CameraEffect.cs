using UnityEngine;
using Cinemachine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] private float _minVelocity = 30;
    [SerializeField] private float _maxVelocity = 70;

    [Header("FOV Parameter :")]
    [SerializeField] private float _maxFov = 100;
    [SerializeField] private float _minFov = 70;

    [Header("Camera Shake :")]
    [SerializeField] private float _maxAmplitude = 2;
    [SerializeField] private float _maxFrequency = 0;

    private CinemachineVirtualCamera _virtualCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    void Start()
    {
        _virtualCam = GameObject.FindGameObjectWithTag("CameraSetup").GetComponentInChildren<CinemachineVirtualCamera>();
        _perlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetCameraFovWithVelocity(float velocityMag)
    {
        float newFov = Mathf.Lerp(_minFov, _maxFov, Mathf.InverseLerp(_minVelocity, _maxVelocity, velocityMag));
        _virtualCam.m_Lens.FieldOfView = newFov;
    }

    public void ShakeCameraWithVelocity(float velocityMag)
    {
        float magValue = Mathf.InverseLerp(_minVelocity, _maxVelocity, velocityMag);
        _perlin.m_AmplitudeGain = Mathf.Lerp(0, _maxAmplitude, magValue);
        _perlin.m_FrequencyGain = Mathf.Lerp(0, _maxFrequency, magValue);
    }
}
