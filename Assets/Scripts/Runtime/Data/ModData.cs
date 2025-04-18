using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exprite;

namespace Incredimaker
{
    // ===== Performer Data JSON =====

    [CreateAssetMenu(fileName = "PerformerData", menuName = "Incredimaker/Performer Data")]
    public class PerformerData : ScriptableObject
    {
        [HideInInspector] public string version;
        public string name;
        public string description;
        public PerformerSong song;
        public PerformerAnimation animation;
        public PerformerIcon icon;

        public static PerformerData FromJson(string json)
        {
            PerformerData data = ScriptableObject.CreateInstance<PerformerData>();
            JsonUtility.FromJsonOverwrite(json, data);

            // Load the audio clips
            foreach (var loop in data.song.loops)
            {
                if (loop.clip == null)
                {
                    loop.clip = FileLoad.GetAudioClipFromPath(loop.file);
                    if (loop.clip == null)
                    {
                        Debug.LogError($"Failed to load audio clip from path: {loop.file}");
                    }
                }
            }
            return data;
        }
    }

    [Serializable]
    public class PerformerSong
    {
        public int BPM;
        public List<PerformerSongLoop> loops;
    }

    [Serializable]
    public class PerformerSongLoop
    {
        [HideInInspector] public string file;
        public AudioClip clip;
        public int startMeasure;
        public int length;
    }

    [Serializable]
    public class PerformerAnimation
    {
        [HideInInspector] public string type;
        [HideInInspector] public string path;
        public ExpriteAnimationPack animationPack;
    }

    [Serializable]
    public class PerformerIcon
    {
        [HideInInspector] public string available;
        public Texture2D availableTexture;
        [HideInInspector] public string used;
        public Texture2D usedTexture;
    }

    // ===== Mod Data JSON =====
    public class ModData
    {
        public string version;
        public string name;
        public string description;
    }
}