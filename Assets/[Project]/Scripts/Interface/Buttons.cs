using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject _optionCanvas;
     
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

    public void OpenOptions()
    {
        // print("OpenOptions");
        _optionCanvas.SetActive(true);
        // Debug.Log("Option!");
    }

    public void CloseOptions()
    {
        print("CloseOptions");
        _optionCanvas.SetActive(false);
        Debug.Log("Close!");
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

    public void Playmode()
    {
        SceneManager.LoadScene("Integration");
        Debug.Log("Play_Mode!");
    }

    public void Debugmode()
    {
        SceneManager.LoadScene("Debug");
        Debug.Log("Debug_Mode!");
    }
}
