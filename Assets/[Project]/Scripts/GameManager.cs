using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool CanPlayerMove => _canPlayerMove;
    public bool IsGamePause => _isGamePause;

    private bool _canPlayerMove = true;
    private bool _isGamePause = false;
    private GameObject _player;
    private CameraControler _cameraControler;


    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraControler = _player.GetComponent<CameraControler>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayerControleAbility(bool value)
    {
        _canPlayerMove = value;
        _player.GetComponent<PlayerInput>().enabled = value;
    }

    private void OnPauseGame(InputValue inputValue)
    {
        _isGamePause = !_isGamePause;

        _player.GetComponent<PlayerInput>().enabled = !_isGamePause;
        Time.timeScale = _isGamePause ? 0 : 1;

        CanvasManager.instance.SetPauseGame(_isGamePause);
        MenuManager.instance.SetFirstSelectedObject(MenuManager.instance.FirstPauseButton);
    }
}
