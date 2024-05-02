using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class ScriptableQuest : ScriptableObject
{
    public string type;
    public string title;
    public string description;
    [Space]
    public string QUEST_ID;
    public bool isQuestDone = false;
}
