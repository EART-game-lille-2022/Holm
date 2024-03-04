using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;

    public void PassOnPanelMode()
    {
        GameManager.instance.SetPlayerToQuestPanelState(_cameraTarget);
    }
}
