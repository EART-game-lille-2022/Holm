using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission_", menuName = "Data/Mission_Objective", order = 1)]
public class S_MissionObjective : ScriptableObject
{
    public virtual bool CheckFinish()
    {
        return false;
    }
    public virtual void Start()
    {
        Debug.Log("Start objct");
    }

    public virtual void End()
    {
        Debug.Log("End objct");
    }
}
