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
        {
            if (item.QUEST_ID == _currentQuest.QUEST_ID)
            {
                item.gameObject.SetActive(true);
                item.hasBeenTaken = false;
            }
        }
    }

    public void CheckAllQuestCollectible(string ID, ScriptableDialogue dialogue)
    {
        if(!_currentQuest)
            return;

        foreach (var item in _collectibleList)
            if (item.QUEST_ID == ID)
                if (item.hasBeenTaken == false)
                    return;

        print("GG ta fini !");
        DialogueManager.instance.PlayDialogue(dialogue);
        CanvasManager.instance.ClearQuestInformation();
        _currentQuest = null;
    }
}
