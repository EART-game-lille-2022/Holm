using System.Collections.Generic;
using UnityEngine;

public class InteractibleManager : MonoBehaviour
{
    public static InteractibleManager instance;
    [Header("Parameter :")]
    [SerializeField] private float _minimumDistanceToInteract;
    [SerializeField] private List<Interactible> _interactibleList = new List<Interactible>();

    private Interactible _selected;
    private Interactible _lastFramSelected;
    private bool _canPlayerInteract = true;
    private Transform _playerTransform;

    void Awake()
    {
        instance = this;
        if (_interactibleList.Count > 0)
        {
            print("Clean inter list");
            _interactibleList.Clear();
        }
    }

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetInteractibleCapability(bool value)
    {
        _canPlayerInteract = value;
    }

    public void AddInteractible(Interactible toAdd)
    {
        // print(toAdd.name);
        if (!_interactibleList.Contains(toAdd))
            _interactibleList.Add(toAdd);
    }

    public void RemoveInteractible(Interactible toRemove)
    {
        if (_interactibleList.Contains(toRemove))
            _interactibleList.Remove(toRemove);
    }

    private Interactible GetNearestInteractible(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        Interactible toReturn = null;
        foreach (var item in _interactibleList)
        {
            float distance = Vector3.Distance(position, item.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (distance < _minimumDistanceToInteract)
                    toReturn = item;
            }
        }

        return toReturn;
    }

    void Update()
    {
        if (_interactibleList.Count <= 0)
            return;

        _selected = GetNearestInteractible(_playerTransform.position);
        if (_selected != _lastFramSelected)
        {
            _selected?.OnSelected();
            _lastFramSelected?.OnUnselected();
        }

        // DebugInteractibleSelection(_selected);
        _lastFramSelected = _selected;
    }

    private void OnInteract()
    {
        if(GameManager.instance.IsGamePause)
            return;

        if (_selected && _canPlayerInteract)
        {
            _selected.Interact();
        }
    }

    public void OnEndInteraction()
    {
        print("Interaction End !");
        _canPlayerInteract = true;
    }

    private void DebugInteractibleSelection(Interactible nearest)
    {
        if (_interactibleList.Count <= 0)
        {
            print("gjnklsdfbdsldddddnslkjdfngklsjndglkjsndfgklsjndfgklsjdnfgklsjdnfgklsjdnfgklsjnfgklsdjnfgikljn");
            return;
        }

        foreach (var item in _interactibleList)
            Debug.DrawLine(_playerTransform.position, item.transform.position, Color.red);

        if (nearest)
            Debug.DrawLine(_playerTransform.position, nearest.transform.position, Color.green);
    }
}
