using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadExternalAudio : MonoBehaviour
{
    // Make stuff static so we can access it from other scripts

    public static AudioClip LoadAudioClip(string path)
    {
        // Load audio clip from path
        WWW www = new WWW("file://" + path);
        while (!www.isDone) { }
        return www.GetAudioClip();
    }
}
