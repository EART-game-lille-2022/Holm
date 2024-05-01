using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoinAT : MonoBehaviour
{
    public Transform targetObject; // L'objet que la flèche doit pointer
    public float rotationSpeed = 5f; // Vitesse de rotation de la flèche

    void Update()
    {
        // Vérifie si l'objet cible est défini
        if (targetObject != null)
        {
            // Calcul de la direction vers l'objet cible
            Vector3 directionToTarget = targetObject.position - transform.position;

            // Calcul de la rotation nécessaire pour faire pointer la flèche vers l'objet cible
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Rotation progressive de la flèche vers la rotation cible
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("L'objet cible n'est pas défini !");
        }
    }
}
