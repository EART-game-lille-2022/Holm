using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractibleManager : MonoBehaviour
{
    [SerializeField] public static List<Interactible> InteractibleList = new List<Interactible>();
    public static void AddInteractible(Interactible toAdd)
    {
        if(!InteractibleList.Contains(toAdd))
            InteractibleList.Add(toAdd);
    }
    public static void RemoveInteractible(Interactible toRemove)
    {
        if (InteractibleList.Contains(toRemove))
            InteractibleList.Remove(toRemove);
    }


    [Header("Reference :")]
    [SerializeField] private Transform _player;

    [Header("Parameter :")]
    [SerializeField] private float _minimumDistanceToInteract;

    private Interactible _selected;
    private Interactible _lastFramSelected;

    void Awake()
    {
        //TODO Call nello, list static clear pas quand la scene reset ?
        //? lié au disable realode domaine ?
        if (InteractibleList.Count > 0)
            InteractibleList.Clear();
    }

    private Interactible GetNearestInteractible(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        Interactible toReturn = null;
        foreach (var item in InteractibleList)
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
        if (InteractibleList.Count <= 0)
            return;

        _selected = GetNearestInteractible(_player.position);
        if (_selected != _lastFramSelected)
        {
            _selected?.OnSelected();
            _lastFramSelected?.OnUnselected();
        }

        DebugInteractibleSelection(_selected);
        _lastFramSelected = _selected;
    }

    private void DebugInteractibleSelection(Interactible nearest)
    {
        if (InteractibleList.Count <= 0)
            return;

        foreach (var item in InteractibleList)
            Debug.DrawLine(_player.position, item.transform.position, Color.red);

        if (nearest)
            Debug.DrawLine(_player.position, nearest.transform.position, Color.green);
    }

    private void OnInteract()
    {
        if (_selected)
            _selected.Interact();
    }
}
