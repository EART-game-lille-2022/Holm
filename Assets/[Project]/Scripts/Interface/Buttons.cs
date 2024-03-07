using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject _optionCanvas;
     [SerializeField] private GameObject _pauseMenu;
    public void Jouer()
    {
        SceneManager.LoadScene("SceneDev");
        Debug.Log("Play_Game!");
    }

    public void Quitter()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public void OpenPause()
    {
        print("OpenPause");
        _pauseMenu.SetActive(true);
    }

    public void ClosePause()
    {
        print("ClosePause");
        _pauseMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        print("OpenOptions");
        _optionCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        print("CloseOptions");
        _optionCanvas.SetActive(false);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Scene_Credit");
        Debug.Log("Credit!");
    }

    public void RetourMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Return_Menu!");
    }
}
