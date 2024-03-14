using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [Header("Quest Reference : ")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questType;
    [SerializeField] private TextMeshProUGUI _questDescription;


    void Awake()
    {
        instance = this;
    }

    public void SetQuestInformation(ScriptableQuest quest)
    {
        // _questTitle.text = quest.t
    }

}
