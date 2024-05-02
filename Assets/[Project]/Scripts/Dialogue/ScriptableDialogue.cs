using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum PnjMood
{
    None,
    Happy,
    Sad,
    Angry,
    Surprised,
    Hmmm,
}

[Serializable]
public class DialogueState
{
    public PnjMood mood;
    public ScriptablePNJ pnj;
    [TextArea] public string text;

    public Sprite GetImage(out PnjMood currentMood)
    {
        currentMood = mood;
        switch (mood)
        {
            case PnjMood.Happy :
                return pnj.happy != null ? pnj.happy : null;

            case PnjMood.Sad :
                return pnj.sad != null ? pnj.sad : null;

            case PnjMood.Surprised :
                return pnj.surprised != null ? pnj.surprised : null;
            
            case PnjMood.Angry :
                return pnj.angry != null ? pnj.angry : null;

            case PnjMood.Hmmm :
                return pnj.hmmm != null ? pnj.hmmm : null;
        }
        // Debug.LogWarning("PNJ mood not set !!!");
        return null;
    }
}

[CreateAssetMenu(fileName = "Dialogue")]
public class ScriptableDialogue : ScriptableObject
{
    // public bool hasBeenPlayed = false;
    public List<DialogueState> stateList;
}
