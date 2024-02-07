using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDescriptor : MonoBehaviour
{
    public ScriptableDialogue _dialogue;

    public void PlayDialogue()
    {
        DialogueManager.instance.PlayDialogue(_dialogue);
    }
}
