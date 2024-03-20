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
    [TextArea] public string endText;
    [Space]
    public string QUEST_ID;
}
