using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private float _globalVolume = .4f;

    [Header("FX :")]
    [SerializeField] private AudioSource _fxAudioSource;
    [SerializeField] private AudioClip _pauseMenuSound;
    public AudioClip PauseMenuSound => _pauseMenuSound;
    [SerializeField] private AudioClip _finishQuestSound;
    public AudioClip FinishQuestSound => _finishQuestSound;

    [Header("Wind Sound On Speed Parametre :")]
    [SerializeField] private float _windSpeedMin;
    [SerializeField] private float _windSpeedMax;
    [SerializeField] private AudioSource _windLightSource;
    [SerializeField] private AudioSource _windHeavySource;
    [SerializeField] private AnimationCurve _lightWindVolumeCurve;
    [SerializeField] private AnimationCurve _heavyWindVolumeCurve;

    private Rigidbody _playerRigidbody;

    void Awake()
    {
        instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        _windLightSource.volume = 0;
        _windHeavySource.volume = 0;
    }

    void Update()
    {
        SetWindSoundFxWithSpeed(_playerRigidbody.velocity.magnitude);
    }

    public void SetWindSoundFxWithSpeed(float value)
    {
        // if(value < _windSpeedMin)
        // {
        //     _windLightSource.volume = 0;
        //     _windHeavySource.volume = 0;
        //     return;
        // }

        float volumeOnSpeed = Mathf.InverseLerp(_windSpeedMin, _windSpeedMax, value);
        // print(volumeOnSpeed);

        // _windLightSource.volume = Mathf.InverseLerp(1, 0, _lightWindVolumeCurve.Evaluate(volumeOnSpeed)) * _globalVolume;
        // _windLightSource.volume = Mathf.InverseLerp(1, 0, volumeOnSpeed) * _globalVolume;
        _windLightSource.volume = _lightWindVolumeCurve.Evaluate(volumeOnSpeed) * _globalVolume;
        _windHeavySource.volume = _heavyWindVolumeCurve.Evaluate(volumeOnSpeed) * _globalVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        _fxAudioSource.PlayOneShot(clip);
    }
}
