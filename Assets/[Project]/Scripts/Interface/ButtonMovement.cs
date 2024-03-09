using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMovement : MonoBehaviour
{
     public float floatSpeed = 1.0f; // Vitesse du flottement
    public float floatHeight = 10.0f; // Hauteur maximale du flottement

    private Vector3 originalPosition;
    private int floatDirection; // 1 ou -1 pour déterminer la direction du mouvement

    private void Start()
    {
        originalPosition = transform.position;

        // Déterminez la direction du mouvement en fonction de la parité de l'index
        floatDirection = (transform.GetSiblingIndex() % 2 == 0) ? 1 : -1;
    }

    private void Update()
    {
        // Utilisation de Mathf.Sin pour créer un mouvement de va-et-vient
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight * floatDirection;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
