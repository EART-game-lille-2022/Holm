using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [SerializeField] private List<ScriptableQuest> _questList;
    [SerializeField] private ScriptableQuest _currentQuest;
    [SerializeField] private List<Collectible> _collectibleList;
    [SerializeField] private List<QuestTarget> _targetList;
    [SerializeField] private string _popupMessageQuestAlreadyDone;

    //! OnQuestStart est call dans OnQuestStartEvent() quand une quete commence
    //! il renvois la list des collectible lié a la quete en cours
    public UnityEvent<List<Collectible>, QuestTarget> OnQuestStart;

    //! OnQuestEnd est caLL dans FinishQuest() quand une quete est fini
    public UnityEvent OnQuestEnd;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetAllQuest();
    }

    private void OnQuestStartEvent(string questID)
    {
        // print("OnQuestStartEvent Setup");
        List<Collectible> collectibleToSend = new List<Collectible>();
        QuestTarget targetToSend = null;

        foreach (var item in _collectibleList)
        {
            if (item.QUEST_ID == questID)
                collectibleToSend.Add(item);
        }

        foreach (var item in _targetList)
        {
            foreach (var questData in item.DataList)
            {
                if (questData.ID == questID)
                {
                    targetToSend = item;
                    break;
                }
            }
        }

        // print("OnQuestStart Call !");
        OnQuestStart.Invoke(collectibleToSend, targetToSend);
    }

    public void AddCollectible(Collectible toAdd)
    {
        if (!_collectibleList.Contains(toAdd))
            _collectibleList.Add(toAdd);
    }

    public void AddTaret(QuestTarget toAdd)
    {
        if (!_targetList.Contains(toAdd))
            _targetList.Add(toAdd);
    }

    public void SelectQuest(ScriptableQuest quest)
    {
        if (quest.isQuestDone)
        {
            CanvasManager.instance.PrintPopup(_popupMessageQuestAlreadyDone);
            return;
        }

        _currentQuest = quest;
        CanvasManager.instance.SetQuestInformation(quest);
        TurnOnCollectible();
        OnQuestStartEvent(quest.QUEST_ID);
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

        _currentQuest.isQuestDone = true;
        CanvasManager.instance.EndQuestAnimation(_currentQuest);
        CanvasManager.instance.ClearQuestInformation();

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
                    _currentQuest = null;
                }
            });
        }
        else
        {
            _currentQuest = null;
        }

        OnQuestEnd.Invoke();

        if (IsAllQuestDone())
        {
            if(DialogueManager.instance.IsOnDialogue())
            {
                DialogueManager.instance.onEnd.AddListener(EndGame);
            }
            
            EndGame();
        }
    }

    public void EndGame()
    {
        StartCoroutine(GameManager.instance.EndGame());
    }

    public void ResetAllQuest()
    {
        foreach (var item in _questList)
            item.isQuestDone = false;
    }

    public bool IsAllQuestDone()
    {
        foreach (var item in _questList)
            if (!item.isQuestDone)
                return false;

        return true;
    }

    public bool HasCurrentQuest()
    {
        return _currentQuest;
    }
}
