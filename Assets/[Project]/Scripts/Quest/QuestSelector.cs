using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private ScriptableQuest _quest;
    private TextMeshPro _text;
    public ScriptableQuest Quest => _quest;

    void Start()
    {
        if(!_quest)
            return;
            
        _text = GetComponentInChildren<TextMeshPro>();
        _text.text = _quest.title;
    }
}
