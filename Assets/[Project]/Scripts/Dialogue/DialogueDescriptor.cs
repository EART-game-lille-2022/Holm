using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDescriptor : MonoBehaviour
{
    [SerializeField] private ScriptableDialogue _questDialogue;
    [SerializeField] private List<ScriptableDialogue> _dialogueList;
    [SerializeField] private int _indexDialogueToPlay;

    public void PlayDialogue()
    {
        DialogueManager.instance.PlayDialogue(_dialogueList[_indexDialogueToPlay], this);
    }

    public void OnDialogueEnd()
    {
        print("End Dialolgue");
        _indexDialogueToPlay++;
        if(_indexDialogueToPlay >= _dialogueList.Count)
            _indexDialogueToPlay = 0;
    }
}
