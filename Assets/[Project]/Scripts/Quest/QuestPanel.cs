using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private List<QuestSelector> _questSelectorList;
    [Space]
    [SerializeField] private string _popupMessage;
    private List<MeshOutline> _questOutlineList = new List<MeshOutline>();
    private int _selectorIndex = 0;
    private PlayerInput _panelInput;
    private bool _canChangeIndex = true;
    private MeshOutline _meshOutline;

    void Start()
    {
        _panelInput = GetComponent<PlayerInput>();
        GetComponent<Interactible>()._onInteract.AddListener(OpenPanel);
        _meshOutline = GetComponent<MeshOutline>();
        _panelInput.enabled = false;
        _canChangeIndex = true;
        foreach (var item in _questSelectorList)
            _questOutlineList.Add(item.GetComponent<MeshOutline>());
    }

    private void SelecteQuest()
    {
        if (_questSelectorList[_selectorIndex].Quest)
        {
            // print(_questSelectorList[_selectorIndex].Quest.name);
            QuestManager.instance.SelectQuest(_questSelectorList[_selectorIndex].Quest);
            ClosePanel();
        }
        else
            print("No Quest Set");
    }

    // public void SetPanelSetup(bool openQuestPanel)
    // {
    //     if (QuestManager.instance.HasCurrentQuest())
    //         return;

    //     GameManager.instance.SetPlayerControleAbility(!openQuestPanel);
    //     _panelInput.enabled = openQuestPanel;

    //     //! Hide Player meshs;
    //     foreach (var item in _player.GetComponentsInChildren<SkinnedMeshRenderer>())
    //         item.enabled = !openQuestPanel;

    //     if (openQuestPanel)
    //     {
    //         _player.GetComponent<CameraControler>().SetCameraTaret(_cameraTarget);
    //         _selectorIndex = 0;
    //         OverSelector(_selectorIndex, _selectorIndex);
    //     }

    //     if (!openQuestPanel)
    //     {
    //         _player.GetComponent<CameraControler>().ResetCameraTarget();
    //         foreach (var item in _questOutlineList)
    //             item.HideOutline();
    //     }
    // }

    public void OpenPanel()
    {
        if (QuestManager.instance.HasCurrentQuest())
        {
            CanvasManager.instance.PrintPopup(_popupMessage);
            return;
        }

        GameManager.instance.SetPlayerControleAbility(false);
        _panelInput.enabled = true;
        _meshOutline.OnUnselected();

        //! Hide Player meshs;
        foreach (var item in _player.GetComponentsInChildren<SkinnedMeshRenderer>())
            item.enabled = false;

        _player.GetComponent<CameraControler>().SetCameraTaret(_cameraTarget);
        _selectorIndex = 0;
        foreach (var item in _questOutlineList)
            item.OnUnselected();
        OverSelector(0, 0);
    }

    public void ClosePanel()
    {
        GameManager.instance.SetPlayerControleAbility(true);
        _panelInput.enabled = false;

        //! Show Player meshs;
        foreach (var item in _player.GetComponentsInChildren<SkinnedMeshRenderer>())
            item.enabled = true;

        _player.GetComponent<CameraControler>().ResetCameraTarget();
        foreach (var item in _questOutlineList)
            item.HideOutline();
    }

    public void OverSelector(int index, int lastIndex)
    {
        _questOutlineList[lastIndex].OnUnselected();
        _questOutlineList[index].OnSelected();
    }

    private void OnDirection(InputValue inputValue)
    {
        Vector2 inputVector = inputValue.Get<Vector2>();

        if (inputVector.magnitude > .9 && _canChangeIndex)
        {
            _canChangeIndex = false;
            int lastIndex = _selectorIndex;
            _selectorIndex = (_selectorIndex + (int)Mathf.Sign(inputVector.x)) % _questSelectorList.Count;
            //! % boucle au dessut de 0

            //! boucle si plus petit que 0
            if (_selectorIndex < 0)
                _selectorIndex = _questSelectorList.Count - 1;

            OverSelector(_selectorIndex, lastIndex);
            return;
        }

        if (inputVector.magnitude < .1)
            _canChangeIndex = true;
        // print(inputVector.magnitude);
    }

    private void OnSelecte(InputValue inputValue)
    {
        // print("A : " + inputValue.Get<float>());
        bool input = inputValue.Get<float>() > .9f ? true : false;
        if (input)
            SelecteQuest();
    }

    private void OnBack(InputValue inputValue)
    {
        bool input = inputValue.Get<float>() > .9f ? true : false;
        if (input)
            ClosePanel();

        InteractibleManager.instance.SetInteractibleCapability(true);
    }
}
