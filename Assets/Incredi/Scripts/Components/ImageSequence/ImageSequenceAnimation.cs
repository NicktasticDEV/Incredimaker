using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

[System.Serializable]
public class ImageSequenceAnimation
{
    public string name; // Name of the animation that will be used to call it
    public string path;
    public int frameRate;
    public bool loop;
    public float[] offset;
    public float scale;
    public List<Texture2D> frames;

    public ImageSequenceAnimation(string name, string path, int frameRate, bool loop, float[] offset, float scale)
    {
        this.name = name;
        this.path = path;
        this.frameRate = frameRate;
        this.loop = loop;
        this.offset = offset;
        this.scale = scale;
        this.frames = loadFrames();
    }

    public List<Texture2D> loadFrames()
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Image sequence path is empty.");
            return null;
        }
        if (!System.IO.Directory.Exists(path))
        {
            Debug.LogError($"Image sequence path does not exist at: {path}");
            return null;
        }

        Debug.Log($"Loading frames from path: {path}");

        string[] filePaths = System.IO.Directory.GetFiles(path, "*.png");
        filePaths = filePaths.OrderBy(path => path).ToArray(); // Sort the file paths

        if (filePaths.Length == 0)
        {
            Debug.LogError($"No frames found in the image sequence path: {path}");
            return null;
        }

        frames = new List<Texture2D>(filePaths.Length);

        for (int i = 0; i < filePaths.Length; i++)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePaths[i]);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            frames.Add(texture);
        }

        return frames;
    }

}
