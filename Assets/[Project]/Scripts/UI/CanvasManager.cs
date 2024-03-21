using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private Canvas _canvasPauseMenu;

    [Header("Quest Info Pause Menu Reference : ")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questType;
    [SerializeField] private TextMeshProUGUI _questDescription;

    [Header("Quest End Panel : ")]
    [SerializeField] private Canvas _questEndCanvas;
    [SerializeField] private TextMeshProUGUI _questEndText;



    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetPauseGame(false);
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
        _questEndText.text = quest.endText;
        StartCoroutine(DelayAndOff());

        IEnumerator DelayAndOff()
        {
            yield return new WaitForSeconds(4);
            _questEndCanvas.gameObject.SetActive(false);
        }
    }

    public void SetPauseGame(bool value)
    {
        //TODO Animate pause UI
        _canvasPauseMenu.gameObject.SetActive(value);
    }
}
