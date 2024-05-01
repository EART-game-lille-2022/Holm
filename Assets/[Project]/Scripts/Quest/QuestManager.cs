using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [SerializeField] private ScriptableQuest _currentQuest;
    [SerializeField] private List<Collectible> _collectibleList;
    private CollectibleCompasse _compasse;

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
                // _compasse.AddCollectibleTransform(item.transform);
            }
        }
    }

    public void CheckAllQuestCollectible(QuestTargetData data)
    {
        if (!_currentQuest)
            return;

        {
            int collectibleNumber = 0;
            foreach (var item in _collectibleList)
                if (item.QUEST_ID == data.ID)
                    collectibleNumber++;

            if (collectibleNumber == 0)
            {
                print("THE QUEST WITH ID : | " + data.ID + " | DOES NOT HAVE COLLECTIBLE IN SCENE !");
                return;
            }
        }

        foreach (var item in _collectibleList)
        {
            if (item.QUEST_ID == data.ID)
            {
                if (item.hasBeenTaken == false)
                {
                    print("Not all colecible taken return");
                    return;
                }
            }
        }

        print("Call Finish Quest !");
        FinishQuest(data);
    }

    private void FinishQuest(QuestTargetData data)
    {
        print("Quete Fini : " + _currentQuest.name);


        if (data.dialogue)
        {
            print("Quest Dialogue : " + data.dialogue.name);
            DialogueManager.instance.PlayDialogue(data.dialogue, () =>
            {
                if (data.questAfterDialogue)
                {
                    SelectQuest(data.questAfterDialogue);
                }
                else
                {
                    CanvasManager.instance.EndQuestAnimation(_currentQuest);
                    _currentQuest = null;
                }
            });
        }
        else
        {
            CanvasManager.instance.EndQuestAnimation(_currentQuest);
            _currentQuest = null;
        }

        CanvasManager.instance.ClearQuestInformation();
    }

    public bool HasCurrentQuest()
    {
        return _currentQuest;
    }
}
