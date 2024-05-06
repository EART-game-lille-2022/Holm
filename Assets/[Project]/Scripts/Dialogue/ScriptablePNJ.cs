using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

[CreateAssetMenu(fileName = "¨PNJ")]
public class ScriptablePNJ : ScriptableObject
{
    public Sprite happy;
    public Sprite sad;
    public Sprite surprised;
    public Sprite angry;
    public Sprite hmmm;
}
