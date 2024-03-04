using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "Mission_", menuName = "Data/Mission", order = 1)]
public class S_Mission : ScriptableObject
{
    public string missionName;
    public bool isUnlocked = false;
    public ScriptableDialogue dialogue;

    public S_Mission[] unlockedOnFinish;

    public S_MissionObjective[] objectifList;

    public void StartMission() {
        Debug.Log("Start Mission");
        DialogueManager.instance.PlayDialogue(dialogue);
        foreach(S_MissionObjective objectif in objectifList) {
            objectif.Start();
        }
    }
    public bool CheckFinish() {
        foreach(S_MissionObjective objectif in objectifList) {
            if(!objectif.CheckFinish()) return false;
        }
        return true;
    }
}
