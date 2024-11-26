using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    private AudioSource audioSource;
    [HideInInspector]
    public SpriteRenderer characterImage;
    private ImageSequenceAnimator imageSequenceAnimator;
    [SerializeField]
    private Animator animator;
    [HideInInspector]
    public Collider2D interactCollider;

    public bool characterSet = false;
    public bool readyToPlay = false;
    public bool isPlaying = false;

    private Character _selectedCharacter;
    public Character selectedCharacter
    {
        get { return _selectedCharacter; }
        set
        {
            if (_selectedCharacter != value)
            {
                _selectedCharacter = value;
            }
        }
    }
    

    void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        characterImage = GetComponentInChildren<SpriteRenderer>();
        imageSequenceAnimator = GetComponentInChildren<ImageSequenceAnimator>();
        interactCollider = GetComponentInChildren<Collider2D>();
    }

    public void SetCharacter(Character character)
    {
        imageSequenceAnimator.imageSequenceAnimations = character.animations;
        audioSource.clip = character.voice;
        Stop();

        // Check if character is set to default or none
        if (character.name == "Default" || character.name == "None")
        {
            characterSet = false;
            readyToPlay = false;
        }
        else
        {
            characterSet = true;
            readyToPlay = true;
        }

        selectedCharacter = character;
    }

    public void Play(int startFrame = 0, float audioTime = 0)
    {
        if (readyToPlay)
        {
            isPlaying = true;
            imageSequenceAnimator.PlayAnimation("singing", startFrame);
            audioSource.time = audioTime;
            audioSource.Play();
        }
    }

    public void PlayAtHalfTime()
    {
        if (readyToPlay)
        {
            isPlaying = true;
            imageSequenceAnimator.PlayAnimation("singing", imageSequenceAnimator.GetFrameLength("singing") / 2);
            audioSource.time = audioSource.clip.length / 2;
            audioSource.Play();
        }
    }

    public void Stop()
    {
        isPlaying = false;
        imageSequenceAnimator.PlayAnimation("idle");
        audioSource.Stop();
    }

    public void PlayInto()
    {
        animator.Play("Intro");
    }

    public void Visible(bool visible)
    {
        characterImage.enabled = visible;
    }
}
