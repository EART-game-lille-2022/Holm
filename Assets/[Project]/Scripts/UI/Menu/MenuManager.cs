using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private GameObject _firstSelectedButton;
    [Space]
    [SerializeField] private GameObject _firstSelectedPauseMenu;
    public GameObject FirstPauseButton => _firstSelectedPauseMenu;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(_firstSelectedButton)
            EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }

    public void SetFirstSelectedObject(GameObject toSet)
    {
        EventSystem.current.SetSelectedGameObject(toSet);
    }
}
