using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] private List<MenuElement> _menuElementList;
    private MenuElement _lastElement;
    private int _currentIndex = 0;
    private bool _canChangeIndex = true;
    private Menu _currentMenu;

    void Awake()
    {
        instance = this;
    }

    public void SelectMenu(Menu newMenu)
    {
        _currentMenu = newMenu;
        foreach (var item in newMenu.MenuElements)
            _menuElementList.Add(item);

        foreach (var item in _menuElementList)
            item.UnOver();
        
        _currentIndex = 0;
        _menuElementList[0].Over();
    }

    public void CloseMenu()
    {
        _menuElementList.Clear();
        _currentMenu = null;
    }

    private void OverElement(int currentIndex, int lastIndex)
    {
        print("Over : " +_menuElementList[_currentIndex]);
        _menuElementList[lastIndex].UnOver();
        _menuElementList[_currentIndex].Over();
    }

    private void OnInteract(InputValue inputValue)
    {
        if (_menuElementList.Count == 0)
            return;
        _menuElementList[_currentIndex].Select();
    }

    private void OnMenuDirection(InputValue inputValue)
    {
        if (_menuElementList.Count == 0)
            return;

        Vector2 inputVector = inputValue.Get<Vector2>();
        ComputeIndex(inputVector);
    }

    private void ComputeIndex(Vector2 inputVector)
    {
        inputVector.y *= -1;
        if (inputVector.magnitude > .9 && _canChangeIndex)
        {
            _canChangeIndex = false;
            int lastIndex = _currentIndex;

            //! % boucle au dessut de 0
            if(_currentMenu._isMenuVertical)
                _currentIndex = _currentIndex + 
                (int)Mathf.Sign(_currentMenu._isMenuVertical ? inputVector.y : inputVector.x) 
                % _menuElementList.Count;
            // else
            //     _currentIndex = (_currentIndex + (int)Mathf.Sign(inputVector.x)) % _menuElementList.Count;

            //! boucle en dessout de 0
            if (_currentIndex < 0)
                _currentIndex = _menuElementList.Count - 1;

            OverElement(_currentIndex, lastIndex);
            return;
        }

        if (inputVector.magnitude < .1)
            _canChangeIndex = true;
    }
}
