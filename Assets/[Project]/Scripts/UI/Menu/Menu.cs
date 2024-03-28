using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    [SerializeField] private List<MenuElement> _menuElementList;
    public List<MenuElement> MenuElements => _menuElementList;
    public bool _isMenuVertical;
    
    public void UnSelectMenu()
    {
        foreach (var item in _menuElementList)
            item.UnOver();
    }
}
