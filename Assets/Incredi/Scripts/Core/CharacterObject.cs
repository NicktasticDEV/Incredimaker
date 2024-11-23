using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer characterImage;
    private ImageSequenceAnimator imageSequenceAnimator;

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
                OnSelectedCharacterChanged();
            }
        }
    }
    

    void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        characterImage = GetComponentInChildren<SpriteRenderer>();
        imageSequenceAnimator = GetComponentInChildren<ImageSequenceAnimator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectedCharacterChanged()
    {
        Debug.Log("Selected character changed to: " + selectedCharacter.name);
        imageSequenceAnimator.imageSequenceAnimations = selectedCharacter.animations;
        audioSource.clip = selectedCharacter.voice;
        Stop();

        // Check if character is set to default or none
        if (selectedCharacter.name == "Default" || selectedCharacter.name == "None")
        {
            characterSet = false;
            readyToPlay = false;
        }
        else
        {
            characterSet = true;
            readyToPlay = true;
        }
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
}
