using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public bool DEBUG_StartGameEvent = true;
    [SerializeField] private MenuSetup _menuSetup;

    [Header("On Game Start Element :")]
    [SerializeField] private ScriptableQuest _questOnGameStart;
    [SerializeField] private ScriptableDialogue _dialogueOnStart;

    [Header("On Game Start End Element :")]
    [SerializeField] private ScriptableDialogue _endGameDialogue;

    public bool CanPlayerMove => _canPlayerMove;
    public bool IsGamePause => _isGamePause;

    private bool _canPlayerMove = true;
    private bool _isGamePause = false;
    private GameObject _player;
    private CameraControler _cameraControler;


    void Awake()
    {
        instance = this;
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraControler = _player.GetComponent<CameraControler>();
    }

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        if (DEBUG_StartGameEvent)
        {
            _menuSetup.SetMenu();
            FadeoutScreen.instance.CloudDirectHide();
            FadeoutScreen.instance.FadeScreen(1, 0, 2, () =>
            {
                FadeoutScreen.instance.CloudFade(false, 3);
            });
        }
    }

    public void GoOnDebugScene()
    {
        FadeoutScreen.instance.CloudFade(true, 2, () =>
        {
            FadeoutScreen.instance.FadeScreen(0, 1, 2, () =>
            {
                SceneManager.LoadScene(2);
            });
        });
    }

    public void StartGame()
    {
        DialogueManager.instance.PlayDialogue(_dialogueOnStart, () =>
        {
            QuestManager.instance.SelectQuest(_questOnGameStart);
            SetPlayerControleAbility(true);
        });
    }

    public IEnumerator EndGame()
    {
        //TODO BUG : Se lance meme si le dernier dialogue n'est pas fini
        yield return new WaitForSeconds(30f);
        DialogueManager.instance.PlayDialogue(_endGameDialogue, () =>
        {
            FadeoutScreen.instance.CloudFade(true, 5f, () =>
            {
                QuestManager.instance.ResetAllQuest();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        });
    }

    public void InstantReset()
    {
        FadeoutScreen.instance.CloudFade(true, 5f, () =>
        {
            QuestManager.instance.ResetAllQuest();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void SetPlayerControleAbility(bool value)
    {
        _canPlayerMove = value;
        _player.GetComponent<PlayerInput>().enabled = value;
        _player.GetComponent<PlayerControler>().enabled = value;
        _player.GetComponent<GroundCheck>().enabled = value;
        _player.GetComponent<Rigidbody>().isKinematic = !value;
    }

    private void OnPauseGame(InputValue inputValue)
    {
        if (_menuSetup._isInMenu)
            return;

        _isGamePause = !_isGamePause;

        _player.GetComponent<PlayerInput>().enabled = !_isGamePause;
        Time.timeScale = _isGamePause ? 0 : 1;

        CanvasManager.instance.SetPauseGame(_isGamePause);
        MenuManager.instance.SetFirstSelectedObject(MenuManager.instance.FirstPauseButton);
    }
}
