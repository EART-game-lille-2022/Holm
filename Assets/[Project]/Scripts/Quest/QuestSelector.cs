using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private ScriptableQuest _quest;
    public ScriptableQuest Quest => _quest;
}
