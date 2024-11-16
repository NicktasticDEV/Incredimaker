using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageSequenceAnimator : MonoBehaviour
{
    public List<ImageSequenceAnimation> animations;

    private SpriteRenderer spriteRenderer;
    private Coroutine playCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load all the frames for each animation
        foreach (ImageSequenceAnimation animation in animations)
        {
            animation.frames = animation.loadFrames();
        }
    }

    public void PlayAnimation(string name)
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }

        playCoroutine = StartCoroutine(Play(name));
    }

    IEnumerator Play(string name)
    {
        ImageSequenceAnimation animation = animations.Find(a => a.name == name);
        if (animation == null)
        {
            Debug.LogError("Animation not found.");
            yield break;
        }

        if (animation.frames == null || animation.frames.Count == 0)
        {
            Debug.LogError("No frames found in the image sequence path.");
            yield break;
        }

        int frameIndex = 0;
        float timePerFrame = 1.0f / animation.frameRate;
        float timeAccumulator = 0f;

        while (true)
        {
            timeAccumulator += Time.deltaTime;

            if (timeAccumulator >= timePerFrame)
            {
                timeAccumulator -= timePerFrame;

                if (animation.frames[frameIndex] == null)
                {
                    Debug.LogError($"Frame is null at index: {frameIndex}");
                    yield break;
                }

                spriteRenderer.sprite = Sprite.Create(animation.frames[frameIndex], new Rect(0, 0, animation.frames[frameIndex].width, animation.frames[frameIndex].height), new Vector2(0.5f, 0.5f));

                frameIndex++;
                if (frameIndex >= animation.frames.Count)
                {
                    if (animation.loop)
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

        playCoroutine = null;
    }

}
