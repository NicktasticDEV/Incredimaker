using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparrowAnimation
{
    public string name; // Name of the animation that will be used to call it
    public string path;
    public int frameRate;
    public bool loop;
    public float[] offset;
    public float scale;
    public Texture2D image;
    public TextureAtlas textureAtlas;

    public void loadAnimation(string path)
    {
        // Load the texture atlas as (path/animation.xml)
        string xmlPath = path + "/animation.xml";
        if (!System.IO.File.Exists(xmlPath))
        {
            Debug.LogError($"Animation XML file does not exist at: {xmlPath}");
            return;
        }

        // Load the texture atlas
        textureAtlas = TextureAtlas.Load(xmlPath);
        if (textureAtlas == null)
        {
            Debug.LogError($"Failed to load texture atlas at: {xmlPath}");
            return;
        }

        // Load the image
        string imagePath = path + "/" + textureAtlas.ImagePath;
        if (!System.IO.File.Exists(imagePath))
        {
            Debug.LogError($"Image file does not exist at: {imagePath}");
            return;
        }

        byte[] fileData = System.IO.File.ReadAllBytes(imagePath);
        image = new Texture2D(2, 2);
        image.LoadImage(fileData);
    }


}
