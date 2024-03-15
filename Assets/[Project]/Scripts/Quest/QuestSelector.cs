using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private ScriptableQuest _quest;
    private TextMeshPro _text;
    public ScriptableQuest Quest => _quest;

    void Start()
    {
        _text = GetComponentInChildren<TextMeshPro>();
        _text.text = _quest.title;
    }
}
