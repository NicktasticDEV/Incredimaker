using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerformerAudioStructure", menuName = "Incredimaker/Performer/Audio Structure", order = 1)]
public class PerformerAudioStructure : ScriptableObject
{
    public string version;
    public int bpm;
    public List<LoopPoint> loopPoints;
}

[Serializable]
public class LoopPoint
{
    // Instead of storing the file path, we could instead find the audio from the path provided by the json and store it as an audioclip.
    public string file;
    public int startMeasure;
    public int length;
}