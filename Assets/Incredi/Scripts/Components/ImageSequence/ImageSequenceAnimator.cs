using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageSequenceAnimator : MonoBehaviour
{
    public List<ImageSequenceAnimation> animations; // Old. Use ImageSequenceAnimations instead
    public ImageSequenceAnimations imageSequenceAnimations;

    private SpriteRenderer spriteRenderer;
    private Coroutine playCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAnimation(string name, int startFrame = 0)
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine(Play(name, startFrame));
    }

    public int GetFrameLength(string name)
    {
        ImageSequenceAnimation animation = imageSequenceAnimations.animations.Find(a => a.name == name);
        if (animation == null)
        {
            Debug.LogError("Animation not found.");
            return 0;
        }

        return animation.frames.Count;
    }

    IEnumerator OldPlay(string name)
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

    IEnumerator Play(string name, int startFrame = 0)
    {
        ImageSequenceAnimation animation = imageSequenceAnimations.animations.Find(a => a.name == name);
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

        int frameIndex = startFrame;
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

                // Offset the transformation
                float xOffset = imageSequenceAnimations.animations.Find(a => a.name == name).offset[0];
                float yOffset = imageSequenceAnimations.animations.Find(a => a.name == name).offset[1];
                // Scale
                float scale = imageSequenceAnimations.animations.Find(a => a.name == name).scale;

                transform.localPosition = new Vector3(xOffset, yOffset, 0);
                transform.localScale = new Vector3(scale, scale, 1);

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
