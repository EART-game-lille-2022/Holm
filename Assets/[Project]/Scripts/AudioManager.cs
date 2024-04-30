using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField, Range(0, 1)] private float _globalVolume = .4f;
    [SerializeField, Range(0, 1)] private float _musicVolume = .4f;
    [SerializeField, Range(0, 1)] private float _fxVolume = .4f;

    [Header("Music :")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] public AudioClip _menuMusic;
    [SerializeField] public AudioClip _inGameMusic;


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
        // if (instance)
        //     Destroy(gameObject);
        // else
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
    }

    void OnValidate()
    {
        _fxAudioSource.volume = _fxVolume * _globalVolume;
        _musicSource.volume = _musicVolume * _globalVolume;
    }

    public void SetVolume(float volume)
    {
        _fxAudioSource.volume = volume * _fxVolume * _globalVolume;
        _musicSource.volume = volume * _musicVolume * _globalVolume;
    }

    void Start()
    {
        if (PlayerInstance.instance)
            _playerRigidbody = PlayerInstance.instance.GetComponent<Rigidbody>();

        _windLightSource.volume = 0;
        _windHeavySource.volume = 0;
        SetVolume(.5f);
    }

    void Update()
    {
        if (PlayerInstance.instance)
        {
            if (!_playerRigidbody)
                _playerRigidbody = PlayerInstance.instance.GetComponent<Rigidbody>();

            SetWindSoundFxWithSpeed(_playerRigidbody.velocity.magnitude);
        }
    }

    public void SetMusic(AudioClip clipToSet)
    {
        float volumeBackup = _musicSource.volume;
        DOTween.To((time) => _musicSource.volume = time, volumeBackup, 0, 1)
        .OnComplete(() =>
        {
            _musicSource.clip = clipToSet;
            _musicSource.Play();
            _musicSource.volume = volumeBackup;
        });
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
