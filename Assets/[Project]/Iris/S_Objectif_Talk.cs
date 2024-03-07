using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Talk_", menuName = "Data/Objectif : Talk", order = 1)]
public class S_Objectif_Talk : S_MissionObjective
{
    // ref vers un character
    
    public override bool CheckFinish() {
        return false;
    }
    public override void Start() {
    }

    public override void End() {
    }
}
