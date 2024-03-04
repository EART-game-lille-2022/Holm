using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public bool CanPlayerMove => _canPlayerMove;
    private bool _canPlayerMove = true;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayerControleAbility(bool value)
    {
        _canPlayerMove = value;
    }
}
