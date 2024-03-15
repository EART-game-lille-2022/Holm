using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private ScriptableQuest _currentQuest;

    void Awake()
    {
        instance = this;
    }

    public void SelectQuest(ScriptableQuest quest)
    {
        CanvasManager.instance.SetQuestInformation(quest);
    }
}
