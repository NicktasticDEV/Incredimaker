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
    
    void Start()
    {

    }

    // On mouse click
    private void OnMouseDown()
    {
        // Set character to default
        if (selectedCharacter.name != "Default")
        {
            Reset();
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
            int halfFrame = imageSequenceAnimator.GetFrameLength("singing") / 2;
            float halfTime = audioSource.clip.length / 2;

            //Debug.Log($"Playing at half frame: {halfFrame}, half time: {halfTime}");

            imageSequenceAnimator.PlayAnimation("singing", halfFrame);
            audioSource.Play();
            audioSource.time = halfTime;
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

    public void Reset()
    {
        Stop();
        animator.Play("Refresh");
    }

    public void Visible(bool visible)
    {
        characterImage.enabled = visible;
    }
}
