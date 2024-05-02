using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    public List<Collectible> targetList; // La liste des cibles
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
    }

    public void ClearTargets()
    {
        targetList.Clear();
        print("Targets cleared");
    }

    void Update()
    {
        if (targetList != null && targetList.Count > 0)
        {
            // Trouver la cible la plus proche
            Collectible nearestTarget = FindNearestTarget();

            if (nearestTarget != null)
            {
                // Calculer la direction vers la cible la plus proche
                Vector3 directionToTarget = nearestTarget.transform.position - transform.position;

                // Calculer la rotation nécessaire pour pointer la flèche vers la cible
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // Rotation progressive de la flèche vers la rotation cible
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.LogWarning("Aucune cible disponible !");
        }
    }

    // Méthode pour trouver la cible la plus proche
    private Collectible FindNearestTarget()
    {
        Collectible nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collectible target in targetList)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }
}