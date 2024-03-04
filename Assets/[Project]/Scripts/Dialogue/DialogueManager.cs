using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    //! Un dialogue peut etre appeler de deux facon :
    //! Via un DialogueDescriptor
    //! Via MissionStart
    public static DialogueManager instance;
    [SerializeField] private float _charDelay;
    [SerializeField] private TextMeshProUGUI _textBloc;
    [SerializeField] private Image _pnjImage;
    [SerializeField] private ScriptableDialogue _currentDialogue;
    [SerializeField] private int _stateIndex = 0;
    private bool _isCurrentStateFinish = true;
    private DialogueDescriptor _descriptor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        QuitDialogue();
    }

    public void PlayDialogue(ScriptableDialogue toPlay, DialogueDescriptor descriptor = null)
    {
        if(_currentDialogue == toPlay)
            return;

        //* Get la ref du descriptor pour reset le conditionel des interactions
        if(descriptor)
            _descriptor = descriptor;

        GameManager.instance.SetPlayerControleAbility(false);

        gameObject.SetActive(true);
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
    public void NextDialogueState()
    {
        SetDialogueState(_stateIndex);
        _stateIndex++;
    }

    public void SetDialogueState(int index)
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
        gameObject.SetActive(false);
        _textBloc.text = " ";
        _pnjImage.sprite = null;

        _currentDialogue = null;
        _stateIndex = 0;

        _descriptor?.OnDialogueEnd();
        GameManager.instance.SetPlayerControleAbility(true);
    }
}
