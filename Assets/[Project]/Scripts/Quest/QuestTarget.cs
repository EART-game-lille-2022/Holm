using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTarget : MonoBehaviour
{
    public string QUEST_ID;
    public ScriptableDialogue _dialogue;

    public void Interact()
    {
        if(_dialogue)
            DialogueManager.instance.PlayDialogue(_dialogue);
        
        QuestManager.instance.CheckAllQuestCollectible();
    }
}
