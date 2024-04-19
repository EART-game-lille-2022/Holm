using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetup : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _playerTarget;

    private Rigidbody _playerRB;
    private CameraControler _cameraControler;

    void Start()
    {
        print("start");
        _playerRB = PlayerInstance.instance.GetComponent<Rigidbody>();
        _cameraControler = _playerRB.GetComponent<CameraControler>();
    }

    public void SetMenu()
    {
        if(!_playerRB)
            Start();

        AudioManager.instance.SetMusic(AudioManager.instance._menuMusic);
        GameManager.instance.SetPlayerControleAbility(false);

        _playerRB.MovePosition(_playerTarget.position);
        _cameraControler.SetCameraTaret(_cameraTarget);
        _cameraControler.EnableCameraEffect(false);
    }

    public void LeaveMenu()
    {
        _cameraControler.ResetCameraTarget();
    }
}
