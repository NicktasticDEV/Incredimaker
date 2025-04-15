using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerformerData", menuName = "Incredimaker/Performer/Performer Data", order = 1)]
public class Root : ScriptableObject
{
    public string version;
    public string name;
    public string description;
    public Song song;
    public Icon icon;
}

[Serializable]
public class Song
{
    // Instead of storing the file path, we could instead find the audio from the path provided by the json and store it as an audioclip.
    public string type;
    public string path;
}

[Serializable]
public class Icon
{
    // Instead of storing the file path, we could instead find the image from the path provided by the json and store it as a texture2D or whatever.
    public string available;
    public string used;
}
