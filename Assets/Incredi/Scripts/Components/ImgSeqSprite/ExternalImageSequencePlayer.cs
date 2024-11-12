using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class ExternalImageSequencePlayer : MonoBehaviour
{
    public string imageSequencePath;

    public float frameRate = 30.0f;
    public bool loop = true;
    public bool playOnAwake = true;

    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    public Texture2D[] frames;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        loadFrames();

        if (playOnAwake)
        {
            StartCoroutine(play());
        }
    }

    void loadFrames()
    {
        if (string.IsNullOrEmpty(imageSequencePath))
        {
            Debug.LogError("Image sequence path is empty.");
            return;
        }
        if (!Directory.Exists(imageSequencePath))
        {
            Debug.LogError("Image sequence path does not exist.");
            return;
        }

        string[] filePaths = Directory.GetFiles(imageSequencePath, "*.png");
        filePaths = filePaths.OrderBy(path => path).ToArray(); // Sort the file paths

        frames = new Texture2D[filePaths.Length];

        for (int i = 0; i < filePaths.Length; i++)
        {
            byte[] fileData = File.ReadAllBytes(filePaths[i]);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            frames[i] = texture;
        }

        // Set the first frame (Make sure width and height of sprite renderer is set to the width and height of the texture)
        spriteRenderer.sprite = Sprite.Create(frames[0], new Rect(0, 0, frames[0].width, frames[0].height), new Vector2(0.5f, 0.5f));
    }

    IEnumerator play()
    {
        int frameIndex = 0;
        float timePerFrame = 1.0f / frameRate;
        float timeAccumulator = 0f;

        while (true)
        {
            timeAccumulator += Time.deltaTime;

            if (timeAccumulator >= timePerFrame)
            {
                timeAccumulator -= timePerFrame;
                spriteRenderer.sprite = Sprite.Create(frames[frameIndex], new Rect(0, 0, frames[frameIndex].width, frames[frameIndex].height), new Vector2(0.5f, 0.5f));

                frameIndex++;
                if (frameIndex >= frames.Length)
                {
                    if (loop)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return null;
        }
    }

}
