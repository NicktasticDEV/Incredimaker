using System.Collections.Generic;
using UnityEngine;

// Mod metadata format
[System.Serializable]
public class ModMetadata
{
    public string version;
    public string modName;
    public string modDescription;
}

public class PNGSeqAnimation
{
    public string name;
    public string path;
    public int frameRate;
    public bool loop;
    public bool playOnAwake;
}








// Character Data
[System.Serializable]
public class Character
{
    public string name;
    public int measureLength;
    public Texture2D thumbnail;
    public AudioClip voice;
}

[System.Serializable]
public class Mod
{
    public string version;
    public string modName;
    public string modDescription;
    public int songBPM;
    public string path;
    public List<Character> characters;
}


