using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static List<Collectible> collectibles = new List<Collectible>();

    public string objectID;
    private Interactible interactible;

    void Awake()
    {
        collectibles.Add(this);
        gameObject.SetActive(false);

        // GetComponent<Interactible>()._onInteract.AddListener(OnInteract);
    }

    void OnDestroy()
    {
        collectibles.Remove(this);
    }

    public void OnInteract()
    {
        foreach (var mission in QuestManager.instance.currentMissionList)
        {
            print("comp quest : " + mission.missionName);
            foreach (var objectif in mission.objectifList)
            {
                print(objectif.name + " || " + objectID + " || " + (objectif is S_Objectif_Collectible));
                if (objectif is S_Objectif_Collectible && ((S_Objectif_Collectible)objectif).objectID == objectID)
                {
                    objectif.End();
                }
            }
        }
    }
}
