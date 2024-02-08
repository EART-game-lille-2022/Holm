using System;
using System.Collections.Generic;
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
    public string text;

    public Sprite GetImage(out PnjMood currentMood)
    {
        currentMood = mood;
        switch (mood)
        {
            case PnjMood.Happy :
                return pnj.happy;

            case PnjMood.Sad :
                return pnj.sad;

            case PnjMood.Surprised :
                return pnj.surprised;
            
            case PnjMood.Angry :
                return pnj.angry;

            case PnjMood.Hmmm :
                return pnj.hmmm;
        }
        Debug.LogWarning("PNJ mood not set !!!");
        return null;
    }
}

[CreateAssetMenu(fileName = "Dialogue")]
public class ScriptableDialogue : ScriptableObject
{
    public List<DialogueState> stateList;
}