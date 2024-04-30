using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] public UnityEvent _onInteract;
    private MeshOutline _meshOutline;

    void Start()
    {
        _meshOutline = GetComponent<MeshOutline>();
        InteractibleManager.instance.AddInteractible(this);
    }

    public void Interact()
    {
        // print("Interact with : " + name);
        _onInteract.Invoke();
    }

    public void OnSelected()
    {
        _meshOutline.OnSelected();
    }

    public void OnUnselected()
    {
        _meshOutline.OnUnselected();
    }

    void OnDestroy()
    {
        InteractibleManager.instance.RemoveInteractible(this);
    }
}
