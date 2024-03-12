using UnityEngine;

[CreateAssetMenu(fileName = "Mission_", menuName = "Data/Mission", order = 1)]
public class S_Mission : ScriptableObject
{
    public bool isUnlocked = false;
    public string missionName;
    public string missionType;
    [TextArea] public string recap;

    [Space]

    public ScriptableDialogue startDialogue;
    public ScriptableDialogue endDialogue;
    public S_Mission[] unlockedOnFinish;
    public S_MissionObjective[] objectifList;

    public void StartMission()
    {
        Debug.Log("Start Mission");

        DialogueManager.instance.PlayDialogue(startDialogue);
        QuestManager.instance.AddMission(this);

        foreach (S_MissionObjective objectif in objectifList)
            objectif.Start();
    }
    
    public bool CheckFinish()
    {
        foreach (S_MissionObjective objectif in objectifList)
        {
            if (!objectif.CheckFinish()) return false;
        }
        return true;
    }
}
