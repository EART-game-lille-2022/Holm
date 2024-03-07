using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<S_Mission> currentMissionList = new List<S_Mission>();

    void Awake()
    {
        instance = this;
    }

    public void AddMission(S_Mission mission)
    {
        currentMissionList.Add(mission);
        // ajouter a l'UI
    }

    public void FinishMission(S_Mission mission)
    {
        Debug.Log("Mission Finished : " + mission.missionName);
        currentMissionList.Remove(mission);
        // ajouter a l'UI
    }

    void Update()
    {
        List<S_Mission> missionToRemove = new List<S_Mission>();

        foreach (S_Mission s_mission in currentMissionList)
        {
            if (s_mission.CheckFinish())
                missionToRemove.Add(s_mission);
        }

        if(missionToRemove.Count != 0)
            foreach (var item in missionToRemove)
                currentMissionList.Remove(item);
    }
}
