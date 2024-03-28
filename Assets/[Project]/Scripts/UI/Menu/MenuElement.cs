using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuElement : MonoBehaviour
{
    [SerializeField] private Color _unSelectedColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.green;
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void Select()
    {
        print(name + " Selected !");
        GetComponent<Button>().onClick.Invoke();
    }

    public void Over()
    {
        _image.color = _selectedColor;
    }

    public void UnOver()
    {
        _image.color = _unSelectedColor;
    }
}