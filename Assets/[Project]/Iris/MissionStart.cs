using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStart : MonoBehaviour
{
    public S_Mission missionStart;

    // IEnumerator Start()
    // {
    //     yield return new WaitForSeconds(5);
    //     missionStart.StartQuest();
    // }

    public void StartQuest()
    {
        missionStart.StartMission();
    }
}
