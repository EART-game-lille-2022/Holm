using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCompasse : MonoBehaviour
{
    [SerializeField] private List<Transform> _colectibleTransformList;

    public void AddCollectibleTransform(Transform toAdd)
    {
        _colectibleTransformList.Add(toAdd);
    }

    public void ResetCollectibleTransform()
    {
        _colectibleTransformList.Clear();
    }
}
