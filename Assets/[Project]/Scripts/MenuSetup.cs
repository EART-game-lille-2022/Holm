using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSetup : MonoBehaviour
{
    [SerializeField] private Canvas _mainMenuCanvas;
    [Space]
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _playerTarget;
    [Space]
    [SerializeField] private Button _menuFirstSelected;
    private Rigidbody _playerRB;
    private CameraControler _cameraControler;
    public bool _isInMenu = true;

    void Start()
    {
        _isInMenu = GameManager.instance.DEBUG_StartGameEvent;
        _mainMenuCanvas.gameObject.SetActive(false);

        _playerRB = PlayerInstance.instance.GetComponent<Rigidbody>();
        _cameraControler = _playerRB.GetComponent<CameraControler>();
        _menuFirstSelected.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        LeaveMenu();
    }

    public void SetMenu()
    {
        _mainMenuCanvas.gameObject.SetActive(true);

        AudioManager.instance.SetMusic(AudioManager.instance._menuMusic);
        GameManager.instance.SetPlayerControleAbility(false);

        _playerRB.MovePosition(_playerTarget.position);

        _cameraControler.SetCameraTaret(_cameraTarget);
        _cameraControler.EnableCameraEffect(false);

        MenuManager.instance.SetFirstSelectedObject(_menuFirstSelected.gameObject);
    }

    public void LeaveMenu()
    {
        AudioManager.instance.SetMusic(AudioManager.instance._inGameMusic);
        AudioManager.instance.PlaySFX(AudioManager.instance.StartGameSound);
        _mainMenuCanvas.gameObject.SetActive(false);

        _cameraControler.LerpCameraToPlayer(_cameraTarget, () =>
        {
            CanvasManager.instance.SetPauseMenu(true);
            _cameraControler.EnableCameraEffect(true);
            _isInMenu = false;

            GameManager.instance.StartGame();
        });
    }
}
