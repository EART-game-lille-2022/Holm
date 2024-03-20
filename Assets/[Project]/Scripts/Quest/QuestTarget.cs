using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestTargetData
{
    public string ID;
    public ScriptableDialogue dialogue;
}

public class QuestTarget : MonoBehaviour
{
    [SerializeField] private List<QuestTargetData> _dataList = new List<QuestTargetData>();

    public void Interact()
    {
        foreach (var item in _dataList)
        {
            QuestManager.instance.CheckAllQuestCollectible(item);
        }

        //TODO avoir un objet qui contient a la fois lq QUEST_ID et un diaogue de fin de mission
        //! le dit dialogue lancer pas le quest manager
    }
}
