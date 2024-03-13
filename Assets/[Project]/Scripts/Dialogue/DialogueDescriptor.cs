using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDescriptor : MonoBehaviour
{
    [SerializeField] private ScriptableDialogue _dialogue;

    void Start()
    {
        _dialogue.hasBeenPlayed = false;
    }

    public void PlayDialogue()
    {
        DialogueManager.instance.PlayDialogue(_dialogue);
    }
}
