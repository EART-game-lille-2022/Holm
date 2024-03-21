using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private Canvas _canvasPauseMenu;
    [Header("In Game :")]
    [SerializeField] private Canvas _canvasInGame;
    [SerializeField] private GameObject _popup;


    [Header("Quest Info Pause Menu Reference : ")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questType;
    [SerializeField] private TextMeshProUGUI _questDescription;

    [Header("Quest End Panel : ")]
    [SerializeField] private Canvas _questEndCanvas;
    [SerializeField] private float _endPanelDuration = 3;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetPauseGame(false);
    }

    public void PrintPopup(string toSay)
    {
        //TODO ajouter une liste de string et lacher les popup dans un par un
        GameObject newPop = Instantiate(_popup, _canvasInGame.transform);
        newPop.GetComponent<TextMeshProUGUI>().text = toSay;

    }

    public void SetQuestInformation(ScriptableQuest quest)
    {
        _questTitle.text = quest.title;
        _questType.text = quest.type;
        _questDescription.text = quest.description;
    }

    public void ClearQuestInformation()
    {
        _questTitle.text = "";
        _questType.text = "";
        _questDescription.text = "";
    }

    public void ActiveEndQuestPanel(ScriptableQuest quest)
    {
        _questEndCanvas.gameObject.SetActive(true);
        StartCoroutine(DelayAndOff());

        IEnumerator DelayAndOff()
        {
            yield return new WaitForSeconds(_endPanelDuration);
            _questEndCanvas.gameObject.SetActive(false);
        }
    }

    public void SetPauseGame(bool value)
    {
        //TODO Animate pause UI
        _canvasPauseMenu.gameObject.SetActive(value);
    }
}
