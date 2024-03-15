using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDescriptor : MonoBehaviour
{
    [SerializeField] private ScriptableDialogue _dialogue;

    void Start()
    {
        if(_dialogue)
            _dialogue.hasBeenPlayed = false;
    }

    public void PlayDialogue()
    {
        print("Play dialogue : " + _dialogue.name);
        DialogueManager.instance.PlayDialogue(_dialogue);
    }
}
