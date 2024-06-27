using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    public List<Collectible> targetList; // La liste des cibles
    public Transform pnjTransform;
    public GameObject arrowObject; // L'objet de la flèche
    public float rotationSpeed = 5f; // Vitesse de rotation de la flèche

    void Start()
    {
        QuestManager.instance.OnQuestStart.AddListener(GetTarget);
        QuestManager.instance.OnQuestEnd.AddListener(ClearTargets);
        arrowObject.SetActive(false);
    }

    public void GetTarget(List<Collectible> targets, QuestTarget questTarget)
    {
        targetList = targets;
        pnjTransform = questTarget.transform;
        print("Targets updated");

        // Activer la flèche si la liste de cibles n'est pas vide
        if (targetList.Count > 0)
        {
            arrowObject.SetActive(true);
        }
    }

    public void ClearTargets()
    {
        targetList.Clear();
        print("Targets cleared");

        // Désactiver la flèche lorsque la liste est vidée
        arrowObject.SetActive(false);
    }

    void Update()
    {
        if (targetList.Count == 0)
            return;

        // Trouver la cible la plus proche qui n'a pas été prise
        Collectible nearestTarget = FindNearestTarget();
        if (!nearestTarget)
        {
            Vector3 directionToPNJ = pnjTransform.position - transform.position;
            // Calculer la rotation nécessaire pour pointer la flèche vers la cible
            Quaternion targetRotationToPNJ = Quaternion.LookRotation(directionToPNJ);
            // Rotation progressive de la flèche vers la rotation cible
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationToPNJ, rotationSpeed * Time.deltaTime);
            return;
        }


        // Calculer la direction vers la cible la plus proche
        Vector3 directionToTarget = nearestTarget.transform.position - transform.position;
        // Calculer la rotation nécessaire pour pointer la flèche vers la cible
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        // Rotation progressive de la flèche vers la rotation cible
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Méthode pour trouver la cible la plus proche qui n'a pas été prise
    private Collectible FindNearestTarget()
    {
        Collectible nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collectible target in targetList)
        {
            // Vérifier si le collectible a été pris
            if (!target.hasBeenTaken)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    nearestTarget = target;
                }
            }
        }

        if (!nearestTarget)
            return null;

        return !nearestTarget.hasBeenTaken ? nearestTarget : null;
    }
}