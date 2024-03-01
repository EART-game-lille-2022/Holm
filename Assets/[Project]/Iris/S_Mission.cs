using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission_", menuName = "Data/Mission", order = 1)]
public class S_Mission : ScriptableObject
{
    public string missionName;
    public bool isUnlocked = false;
    [TextArea] public string dialogue;

    public S_Mission[] unlockedOnFinish;

    public S_MissionObjective[] objectives;

    public void StartQuest() {
        foreach(S_MissionObjective o in objectives) {
            o.Start();
        }
    }
    public bool CheckFinish() {
        foreach(S_MissionObjective o in objectives) {
            if(!o.CheckFinish()) return false;
        }
        return true;
    }
}
