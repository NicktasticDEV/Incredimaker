using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Extrite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SparrowRenderer : MonoBehaviour
    {
        // Public Variables
        public bool isPlaying { get; private set; }
        public Animation? currentAnimation { get; private set; }

        private static Dictionary<SO_SparrowAnimationPack, Dictionary<string, List<Sprite>>> preloadedAnimations = new Dictionary<SO_SparrowAnimationPack, Dictionary<string, List<Sprite>>>();

        // Components
        private SpriteRenderer spriteRenderer;

        // Fields
        public bool preloadAnimationPack = false;

        private SO_SparrowAnimationPack previousSparrowAnimationPack;
        public SO_SparrowAnimationPack sparrowAnimationPack;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            previousSparrowAnimationPack = sparrowAnimationPack;

            if (preloadAnimationPack && sparrowAnimationPack != null)
            {
                PreloadAnimationPack();
            }
        }

        void Start()
        {
            #if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnExitPlayMode;
            #endif 
        }

        #if UNITY_EDITOR
        void OnExitPlayMode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                preloadedAnimations.Clear();
            }
        }
        #endif

        void Update()
        {
            if (previousSparrowAnimationPack != sparrowAnimationPack && sparrowAnimationPack != null)
            {
                OnSparrowAnimationPackChanged();
                previousSparrowAnimationPack = sparrowAnimationPack;
            }
        }

        public void Play(string animationName, int startingFrame = 0)
        {
            if (isPlaying)
            {
                StopAllCoroutines();
            }

            StartCoroutine(PlayAnimation(animationName));
        }

        [ContextMenu("Import Sparrow Animation Pack")]
        public void ImportSparrowAnimationPack()
        {
            string path = Extrite.Utilities.GetPathFromDialogue("Import Sparrow Animation Pack", "esap", false);
            sparrowAnimationPack = Extrite.Utilities.ImportSparrowAnimationPack(path);
        }

        IEnumerator PlayAnimation(string animationName, int startingFrame = 0)
        {
            Animation animation = sparrowAnimationPack.GetAnimationByName(animationName);

            isPlaying = true;
            currentAnimation = animation;

            int frameIndex = startingFrame;
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

                    if (!preloadAnimationPack)
                    {
                        SubTexture subTextureFrame = subTextures[frameIndex];
                        float adjustedY = sparrowAnimationPack.texture.height - subTextureFrame.y - subTextureFrame.height;

                        Vector2 pivot = new Vector2(
                            (subTextureFrame.frameX - animation.offset.x - sparrowAnimationPack.globalOffset.x) / subTextureFrame.width,
                            1f - ((subTextureFrame.frameY + animation.offset.y + sparrowAnimationPack.globalOffset.y) / subTextureFrame.height)
                        );

                        spriteRenderer.sprite = Sprite.Create(
                            sparrowAnimationPack.texture,
                            new Rect(subTextureFrame.x, adjustedY, subTextureFrame.width, subTextureFrame.height),
                            pivot
                        );
                    }
                    else
                    {
                        spriteRenderer.sprite = preloadedAnimations[sparrowAnimationPack][animationName][frameIndex];
                    }

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
    
        void OnSparrowAnimationPackChanged()
        {
            Debug.Log("Sparrow Animation Pack Changed");
            if (preloadAnimationPack)
            {
                PreloadAnimationPack();
            }
        }

        public int GetFrameLength(string animationName)
        {
            Animation animation = sparrowAnimationPack.GetAnimationByName(animationName);
            return sparrowAnimationPack.GetSubTexturesFromAnimation(animation).Length;
        }

        // Preload self animation pack
        void PreloadAnimationPack()
        {
            Debug.Log("Preloading Animation Pack");

            if (!preloadedAnimations.ContainsKey(sparrowAnimationPack))
            {
                Dictionary<string, List<Sprite>> animations = new Dictionary<string, List<Sprite>>();

                foreach (Animation animation in sparrowAnimationPack.animations)
                {
                    List<Sprite> sprites = new List<Sprite>();

                    SubTexture[] subTextures = sparrowAnimationPack.GetSubTexturesFromAnimation(animation);

                    foreach (SubTexture subTexture in subTextures)
                    {
                        float adjustedY = sparrowAnimationPack.texture.height - subTexture.y - subTexture.height;

                        Vector2 pivot = new Vector2(
                            (subTexture.frameX - animation.offset.x - sparrowAnimationPack.globalOffset.x) / subTexture.width,
                            1f - ((subTexture.frameY + animation.offset.y + sparrowAnimationPack.globalOffset.y) / subTexture.height)
                        );

                        Sprite sprite = Sprite.Create(
                            sparrowAnimationPack.texture,
                            new Rect(subTexture.x, adjustedY, subTexture.width, subTexture.height),
                            pivot
                        );

                        sprites.Add(sprite);
                    }

                    animations.Add(animation.name, sprites);
                }

                preloadedAnimations.Add(sparrowAnimationPack, animations);
            }
            else
            {
                Debug.Log("Animation Pack already preloaded");
            }
        }

        // Preload selected animation pack
        public static void PreloadSpecificAnimationPack(SO_SparrowAnimationPack sparrowAnimationPack)
        {
            if (!preloadedAnimations.ContainsKey(sparrowAnimationPack))
            {
                Dictionary<string, List<Sprite>> animations = new Dictionary<string, List<Sprite>>();

                foreach (Animation animation in sparrowAnimationPack.animations)
                {
                    List<Sprite> sprites = new List<Sprite>();

                    SubTexture[] subTextures = sparrowAnimationPack.GetSubTexturesFromAnimation(animation);

                    foreach (SubTexture subTexture in subTextures)
                    {
                        float adjustedY = sparrowAnimationPack.texture.height - subTexture.y - subTexture.height;

                        Vector2 pivot = new Vector2(
                            (subTexture.frameX - animation.offset.x - sparrowAnimationPack.globalOffset.x) / subTexture.width,
                            1f - ((subTexture.frameY + animation.offset.y + sparrowAnimationPack.globalOffset.y) / subTexture.height)
                        );

                        Sprite sprite = Sprite.Create(
                            sparrowAnimationPack.texture,
                            new Rect(subTexture.x, adjustedY, subTexture.width, subTexture.height),
                            pivot
                        );

                        sprites.Add(sprite);
                    }

                    animations.Add(animation.name, sprites);
                }

                preloadedAnimations.Add(sparrowAnimationPack, animations);
            }
            else
            {
                Debug.Log("Animation Pack already preloaded");
            }
        }
    
    }
}