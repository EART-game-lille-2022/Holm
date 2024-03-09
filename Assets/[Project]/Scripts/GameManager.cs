using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool CanPlayerMove => _canPlayerMove;
    private bool _canPlayerMove = true;
    private GameObject _player;
    private CameraControler _cameraControler;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraControler = _player.GetComponent<CameraControler>();
    }

    public void SetPlayerToQuestPanelState(Transform panelPosition)
    {
        SetPlayerControleAbility(false);
        _cameraControler.SetCameraTaret(panelPosition, panelPosition);
    }

    public void SetPlayerToControleState()
    {

    }

    public void SetPlayerControleAbility(bool value)
    {
        _canPlayerMove = value;
    }
}
