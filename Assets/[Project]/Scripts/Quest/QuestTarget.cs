using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestTargetData
{
    public string ID;
    public ScriptableDialogue dialogue;
    public ScriptableQuest questAfterDialogue;
}

public class QuestTarget : MonoBehaviour
{
    [SerializeField] private List<QuestTargetData> _dataList = new List<QuestTargetData>();
    public List<QuestTargetData> DataList {get => _dataList;}

    void Start()
    {
        QuestManager.instance.AddTaret(this);
        GetComponent<Interactible>()._onInteract.AddListener(Interact);
    }

    public void Interact()
    {
        foreach (var item in _dataList)
        {
            QuestManager.instance.CheckAllQuestCollectible(item);
        }
    }
}
