using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField] private Canvas _dialogueCanvas;
    [Space]
    [SerializeField] private float _charDelay;
    [SerializeField] private TextMeshProUGUI _textBloc;
    [SerializeField] private Image _pnjImage;
    [SerializeField] private ScriptableDialogue _currentDialogue;
    [SerializeField] private int _stateIndex = 0;
    private bool _isCurrentStateFinish = true;
    private Action _onEndDialogue;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        QuitDialogue();
    }

    public void PlayDialogue(ScriptableDialogue toPlay, Action toDoAfter = null)
    {
        if(_currentDialogue == toPlay)
            return;

        // if(toPlay.hasBeenPlayed)
        //     return;
    
        if(toDoAfter != null)
            _onEndDialogue = toDoAfter;

        //* Get la ref du descriptor pour reset le conditionel des interactions


        _dialogueCanvas.gameObject.SetActive(true);
        GameManager.instance.SetPlayerControleAbility(false);
        InteractibleManager.instance.SetInteractibleCapability(false);

        _textBloc.text = " ";
        _pnjImage.sprite = null;

        _currentDialogue = toPlay;
        _stateIndex = 0;

        NextDialogueState();
    }
    private void OnInteract(InputValue value)
    {
        // print("Dialogue Interact !");
        if (!_currentDialogue)
            return;


        if (_stateIndex >= _currentDialogue.stateList.Count)
        {
            QuitDialogue();
            return;
        }

        if (!_isCurrentStateFinish)
            SkipPrintingText();
        else
            NextDialogueState();
    }

    private void SkipPrintingText()
    {
        _isCurrentStateFinish = true;
        _textBloc.text = _currentDialogue.stateList[_stateIndex - 1].text;
    }

    [ContextMenu("iuehrgiuhegrhuigreuihegr")]
    private void NextDialogueState()
    {
        SetDialogueState(_stateIndex);
        _stateIndex++;
    }

    private void SetDialogueState(int index)
    {
        _isCurrentStateFinish = false;
        PnjMood pnjMood;
        _pnjImage.sprite = _currentDialogue.stateList[index].GetImage(out pnjMood);
        // print("Mood : " + pnjMood + " at index : " + _stateIndex);
        if(pnjMood == PnjMood.Surprised)
            _pnjImage.transform.DOShakePosition(1, 20, 20, 90);
        StartCoroutine(PrintTexte(_currentDialogue.stateList[index].text));
    }

    IEnumerator PrintTexte(string toPrint)
    {
        _textBloc.text = " ";
        for (int i = 0; i < toPrint.Length && !_isCurrentStateFinish; i++)
        {
            _textBloc.text += toPrint[i];
            yield return new WaitForSeconds(_charDelay);
        }
        _isCurrentStateFinish = true;
    }

    public void QuitDialogue()
    {
        _dialogueCanvas.gameObject.SetActive(false);
        _textBloc.text = " ";
        _pnjImage.sprite = null;

        if(_onEndDialogue != null)
            _onEndDialogue.Invoke();
        _onEndDialogue = null;

        // if(_currentDialogue)
        //     _currentDialogue.hasBeenPlayed = true;
        _currentDialogue = null;
        _stateIndex = 0;

        GameManager.instance.SetPlayerControleAbility(true);
        InteractibleManager.instance.SetInteractibleCapability(true);
    }
}
