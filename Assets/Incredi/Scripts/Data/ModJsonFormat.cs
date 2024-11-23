using System.Collections.Generic;
using UnityEngine;

// Character Data
[System.Serializable]
public class Character
{
    public string name;
    public int measureLength;
    public Texture2D thumbnail;
    public AudioClip voice;
    public ImageSequenceAnimations animations;
}

[System.Serializable]
public class Mod
{
    public string modName;
    public string modDescription;
    public string version;
    public int songBPM;
    public string path;
    public List<Character> characters;
}