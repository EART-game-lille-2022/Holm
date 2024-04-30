using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private Canvas _canvasPauseMenu;

    [Header("In Game :")]
    [SerializeField] private Canvas _canvasInGame;
    [SerializeField] private GameObject _popup;

    [Header("Pause Menu : ")]
    [SerializeField] RectTransform _pauseMenuBackground;

    [Header("Quest Info Pause Menu Reference : ")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questType;
    [SerializeField] private TextMeshProUGUI _questDescription;

    [Header("Quest End Panel : ")]
    [SerializeField] private Canvas _questEndCanvas;
    [SerializeField] private RectTransform _endQuestPanel;
    [SerializeField] private Image _endQuestImage;
    [SerializeField] private TextMeshProUGUI _endQuestTitle;
    [SerializeField] private AnimationCurve _endPanelAlphaCurve;
    [SerializeField] private AnimationCurve _endPanelAnimation;

    private Vector2 _pauseMenuStartPosition;
    private Vector2 _endQuestPanelStartPosition;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _pauseMenuStartPosition = _pauseMenuBackground.anchoredPosition;
        _endQuestPanelStartPosition = _endQuestPanel.anchoredPosition;
        SetPauseGame(false, false);
        ClearQuestInformation();
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
        _questDescription.text = "Tu n'as pas de livraisons en cours. Va au bureau de poste pour en récupérer.";
    }

    public void EndQuestAnimation(ScriptableQuest data)
    {
        _questEndCanvas.gameObject.SetActive(true);

        _endQuestTitle.text = data.title;

        _endQuestPanel.anchoredPosition = new Vector2(0, -800);
        Vector2 startPos = _endQuestPanel.anchoredPosition;
        Color animatedColor = _endQuestImage.color;

        DOTween.To((time) =>
        {
            _endQuestPanel.anchoredPosition =
            Vector2.Lerp(startPos, Vector2.zero, time);

            animatedColor.a = _endPanelAlphaCurve.Evaluate(time);
            _endQuestImage.color = animatedColor;

            Color textColor = _endQuestTitle.color;
            textColor.a = _endPanelAlphaCurve.Evaluate(time);
            _endQuestTitle.color = textColor;
        }, 0, 1, 1)
        .SetEase(_endPanelAnimation)
        .OnComplete(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.FinishQuestSound);
            StartCoroutine(EndPanelDelay(2));
        });
    }

    IEnumerator EndPanelDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Color animatedColor = _endQuestImage.color;
        DOTween.To((time) =>
        {
            _endQuestPanel.anchoredPosition =
            Vector2.Lerp(Vector2.zero, new Vector2(0, 800), time);

            animatedColor.a = Mathf.Lerp(1, 0, _endPanelAlphaCurve.Evaluate(time));
            _endQuestImage.color = animatedColor;

            Color textColor = _endQuestTitle.color;
            textColor.a = Mathf.Lerp(1, 0, _endPanelAlphaCurve.Evaluate(time));
            _endQuestTitle.color = textColor;
        }, 0, 1, 1)
        .SetEase(_endPanelAnimation);
    }

    public void SetPauseGame(bool value, bool playSound = true)
    {
        // print("PAUSE : " + value);
        // _canvasPauseMenu.gameObject.SetActive(value);

        if (playSound)
            AudioManager.instance.PlaySFX(AudioManager.instance.PauseMenuSound);

        Vector2 startAnimationPos = _pauseMenuBackground.anchoredPosition;
        Vector2 target = value ? Vector2.zero : _pauseMenuStartPosition;
        DOTween.To((time) =>
        {
            _pauseMenuBackground.anchoredPosition =
            Vector2.Lerp(startAnimationPos, target, time);
        }, 0, 1, 7)
        .SetEase(Ease.Linear)
        .SetSpeedBased(true)
        .SetUpdate(true);
    }
}
