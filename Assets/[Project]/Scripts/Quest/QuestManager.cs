using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [SerializeField] private ScriptableQuest _currentQuest;
    [SerializeField] private List<Collectible> _collectibleList;

    void Awake()
    {
        instance = this;
    }

    public void AddCollectible(Collectible toAdd)
    {
        if (!_collectibleList.Contains(toAdd))
            _collectibleList.Add(toAdd);
    }

    public void SelectQuest(ScriptableQuest quest)
    {
        _currentQuest = quest;

        CanvasManager.instance.SetQuestInformation(quest);
        TurnOnCollectible();
    }

    private void TurnOnCollectible()
    {
        foreach (var item in _collectibleList)
            item.gameObject.SetActive(false);

        foreach (var item in _collectibleList)
        {
            if (item.QUEST_ID == _currentQuest.QUEST_ID)
            {
                item.gameObject.SetActive(true);
                item.hasBeenTaken = false;
            }
        }
    }

    public void CheckAllQuestCollectible(QuestTargetData data)
    {
        if (!_currentQuest)
            return;

        foreach (var item in _collectibleList)
            if (item.QUEST_ID == data.ID)
                if (item.hasBeenTaken == false)
                    return;

        FinishQuest(data);
    }

    private void FinishQuest(QuestTargetData data)
    {
        print("Quete Fini : " + _currentQuest.name);
        
        DialogueManager.instance.PlayDialogue(data.dialogue, () 
        => { CanvasManager.instance.ActiveEndQuestPanel(_currentQuest); });

        CanvasManager.instance.ClearQuestInformation();

        _currentQuest = null;
    }

    public bool HasCurrentQuest()
    {
        return _currentQuest;
    }
}
