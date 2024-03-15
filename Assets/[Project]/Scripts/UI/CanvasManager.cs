using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] private RectTransform _panelPauseMenu;

    [Header("Quest Reference : ")]
    [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questType;
    [SerializeField] private TextMeshProUGUI _questDescription;

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

    public void SetPauseGame(bool value)
    {
        _panelPauseMenu.gameObject.SetActive(value);
        // Vector2 startPos = _panelPauseMenu.anchoredPosition;
        // Vector2 endPos = Vector2.zero; 
        // if(value)
        //     endPos = new Vector2((1000 / 2) + _panelPauseMenu.rect.width, -1000 / 2);

        // _panelPauseMenu.anchoredPosition = endPos;

            // if (value)
            // {
            //     ((RectTransform)_panelPauseMenu.transform).anchoredPosition = Vector3.zero;
            // }
            // else
            // {
            //     Vector3 newPosition = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
            //     ((RectTransform)_panelPauseMenu.transform).anchoredPosition = newPosition;
            // }
    }
}
