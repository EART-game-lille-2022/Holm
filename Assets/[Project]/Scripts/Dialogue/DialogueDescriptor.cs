using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDescriptor : MonoBehaviour
{
    [SerializeField] private List<ScriptableDialogue> _dialogueList;
    private int _indexDialogueToPlay;
    private Interactible _currentInteractible;

    public void PlayDialogue(Interactible interactible = null)
    {
        if(interactible)
            _currentInteractible = interactible;

        DialogueManager.instance.PlayDialogue(_dialogueList[_indexDialogueToPlay], this);
    }

    public void OnDialogueEnd()
    {
        print("End Dialolgue");
        _indexDialogueToPlay++;
        if(_indexDialogueToPlay >= _dialogueList.Count)
            _indexDialogueToPlay = 0;

        _currentInteractible?.EndInteraction();
    }
}
