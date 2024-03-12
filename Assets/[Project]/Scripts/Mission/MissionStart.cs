using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStart : MonoBehaviour
{
    public S_Mission s_missionToStart;

    void Start()
    {
        GetComponent<Interactible>()._onInteract.AddListener(StartQuest);
        if(s_missionToStart.startDialogue)
            s_missionToStart.startDialogue.hasBeenPlayed = false;
    }

    public void StartQuest()
    {
        s_missionToStart.StartMission();
    }
}
