using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    public List<Collectible> targetList; // La liste des cibles
    public GameObject arrowObject; // L'objet de la flèche
    public float rotationSpeed = 5f; // Vitesse de rotation de la flèche

    void Start()
    {
        QuestManager.instance.OnQuestStart.AddListener(GetTarget);
        QuestManager.instance.OnQuestEnd.AddListener(ClearTargets);
    }

    public void GetTarget(List<Collectible> targets, QuestTarget questTarget)
    {
        targetList = targets;
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
        if (targetList != null && targetList.Count > 0)
        {
            bool allInactive = true; // Variable pour vérifier si tous les collectibles sont inactifs

            // Trouver la cible la plus proche qui n'a pas été prise
            Collectible nearestTarget = FindNearestTarget();

            if (nearestTarget != null)
            {
                // Vérifier si le collectible le plus proche est actif
                if (nearestTarget.gameObject.activeSelf)
                {
                    allInactive = false; // Au moins un collectible est actif

                    // Calculer la direction vers la cible la plus proche
                    Vector3 directionToTarget = nearestTarget.transform.position - transform.position;

                    // Calculer la rotation nécessaire pour pointer la flèche vers la cible
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                    // Rotation progressive de la flèche vers la rotation cible
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }

            // Si tous les collectibles sont inactifs, désactiver la flèche
            if (allInactive)
            {
                arrowObject.SetActive(false);
                Debug.LogWarning("Tous les collectibles sont inactifs !");
            }
        }
        else
        {
            // Désactiver la flèche si la liste de cibles est vide
            arrowObject.SetActive(false);
            Debug.LogWarning("Aucune cible disponible !");
        }
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

        return nearestTarget;
    }
}