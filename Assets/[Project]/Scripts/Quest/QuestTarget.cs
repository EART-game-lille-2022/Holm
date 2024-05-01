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
    //TODO dialogue si all collectible non get
    [SerializeField] private List<QuestTargetData> _dataList = new List<QuestTargetData>();

    void Start()
    {
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
