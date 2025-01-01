using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extrite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SparrowRenderer : MonoBehaviour
    {
        // Public Variables
        public bool isPlaying { get; private set; }
        public Animation? currentAnimation { get; private set; }

        // Components
        private SpriteRenderer spriteRenderer;

        // Fields
        public SO_SparrowAnimationPack sparrowAnimationPack;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play(string animationName)
        {
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            StartCoroutine(PlayAnimation(animationName));
        }

        IEnumerator PlayAnimation(string animationName)
        {
            Animation animation = sparrowAnimationPack.GetAnimationByName(animationName);

            isPlaying = true;
            currentAnimation = animation;

            int frameIndex = 0;
            float timePerFrame = 1f / animation.fps;
            float timeAccumulator = 0f;

            SubTexture[] subTextures = sparrowAnimationPack.GetSubTexturesFromAnimation(animation);

            while (true)
            {
                timeAccumulator += Time.deltaTime;

                // Check if we need to move to the next frame
                if (timeAccumulator >= timePerFrame)
                {
                    timeAccumulator -= timePerFrame;

                    SubTexture subTextureFrame = subTextures[frameIndex];
                    float adjustedY = sparrowAnimationPack.sparrowFilePack.texture.height - subTextureFrame.y - subTextureFrame.height;

                    Vector2 pivot = new Vector2(
                        (subTextureFrame.frameX - animation.offset.x - sparrowAnimationPack.globalOffset.x) / subTextureFrame.width,
                        1f - ((subTextureFrame.frameY + animation.offset.y + sparrowAnimationPack.globalOffset.y) / subTextureFrame.height)
                    );

                    spriteRenderer.sprite = Sprite.Create(
                        sparrowAnimationPack.sparrowFilePack.texture,
                        new Rect(subTextureFrame.x, adjustedY, subTextureFrame.width, subTextureFrame.height),
                        pivot
                    );

                    frameIndex++;

                    // Check if we reached the end of the animation
                    if (frameIndex >= subTextures.Length)
                    {
                        //Check if animation is supposed to loop
                        if (animation.loop)
                        {
                            frameIndex = 0;
                        }
                        else
                        {
                            isPlaying = false;
                            currentAnimation = null;
                            yield break;
                        }
                    }
                }

                yield return null;
            }
        }
    }
}