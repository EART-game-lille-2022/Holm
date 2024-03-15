using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestTarget : MonoBehaviour
{
    public List<string> QUEST_IDLIST = new List<string>();
    public ScriptableDialogue _dialogue;

    public void Interact()
    {
        foreach (var item in QUEST_IDLIST)
            QuestManager.instance.CheckAllQuestCollectible(item, _dialogue);

        //TODO avoir un objet qui contient a la fois lq QUEST_ID et un diaogue de fin de mission
        //! le dit dialogue lancer pas le quest manager
    }
}
