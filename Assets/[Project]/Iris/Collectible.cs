using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static List<Collectible> collectibles = new List<Collectible>();
    
    public string objectID;

    void Awake() {
        collectibles.Add(this);
        gameObject.SetActive(false);
    }

    void OnDestroy() {
        collectibles.Remove(this);
    }
    void Update()
    { 
        // distance avec le joueur -> finish
    }
}
