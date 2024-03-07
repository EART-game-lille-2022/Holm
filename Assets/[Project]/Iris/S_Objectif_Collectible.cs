using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collect_", menuName = "Data/Objectif : Object collect", order = 1)]
public class S_Objectif_Collectible : S_MissionObjective
{
    public string objectID;
    bool hasBeenCollected;

    public override bool CheckFinish()
    {
        return hasBeenCollected;
    }

    public override void Start()
    {
        hasBeenCollected = false;
        foreach (Collectible collectible in Collectible.collectibles)
        {
            if (collectible.objectID == objectID)
            {
                collectible.gameObject.SetActive(true);
            }
        }
    }

    public override void End()
    {
        hasBeenCollected = true;
        foreach (Collectible collectible in Collectible.collectibles)
        {
            if (collectible.objectID == objectID)
            {
                collectible.gameObject.SetActive(false);
            }
        }
    }
}
