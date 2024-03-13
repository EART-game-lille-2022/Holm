using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
}
