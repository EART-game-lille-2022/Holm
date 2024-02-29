using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collect_", menuName = "Data/Objectif : Object collect", order = 1)]
public class S_Objectif_Objects : S_MissionObjective
{
    public string objectID;
    
    public override bool CheckFinish() {
        return false;
    }
    public override void Start() {
        
        foreach(Collectible collectible in Collectible.collectibles) {
            if(collectible.objectID == objectID) {
                collectible.gameObject.SetActive(true);
            }
        }
    }
    
    public override void End() {
    }
}
