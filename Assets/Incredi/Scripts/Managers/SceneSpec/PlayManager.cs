using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    [Header("Settings")]
    [SerializeField] private float randomPosY = 0;
    [SerializeField] private float introTime = 1.5f;

    [Header("Character Objects")]
    public List<CharacterObject> characters = new List<CharacterObject>();

    // Game Variables
    [HideInInspector] public bool songPlaying = false;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public int measureLength = 4;
    [HideInInspector] public bool iconBeingDragged = false;
    [HideInInspector] public string selectedCharacter = "";
    
    void Awake()
    {
        // Make this a singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Subscribe to events
        Metronome.Instance.onMeasure += MeasureHit;

        // Set the BPM of the metronome to the song's BPM
        Metronome.Instance.BPM = ModManager.Instance.mods[0].songBPM;
        Metronome.Instance.defaultBPM = ModManager.Instance.mods[0].songBPM;

        // Set each character to the default character
        foreach (CharacterObject character in characters) { character.SetCharacter(ModManager.Instance.GetCharacter("Default")); }

        // Start the character animation intro
        StartCoroutine(CharacterAnimationIntro());
    }

    void Update()
    {
        // Play song if all characters are ready
        if (!songPlaying && IsAllCharactersReady() && !paused)
        {
            Metronome.Instance.Reset();
            foreach (CharacterObject character in characters)
            {
                character.Play();
            }
            songPlaying = true;
        }
        
        // Stop song if not all characters are ready
        if (!IsAllCharactersReady())
        {
            songPlaying = false;
        }

        // Highlight character that cursor is over
        foreach (CharacterObject character in characters)
        {
            if (character.interactCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && iconBeingDragged)
            {
                character.characterImage.material.SetColor("_TintColor", new Color(1, 1, 1, 0.5f));
            }
            else
            {
                character.characterImage.material.SetColor("_TintColor", new Color(1, 1, 1, 0f));
            }
        }
    }

    void MeasureHit()
    {   
        // Play character on measure hit
        foreach (CharacterObject character in characters)
        {
            // Play character if character is set
            if (character.characterSet)
            {
                // Character with measure length of 1
                if (character.selectedCharacter.measureLength == 1)
                {
                    if (Metronome.Instance.measureCount % 1 == 0)
                    {
                        character.Play();
                    }
                }
                // Character with measure length of 2
                else if (character.selectedCharacter.measureLength == 2)
                {
                    if (Metronome.Instance.measureCount % 2 == 0)
                    {
                        character.Play();
                    }
                }
                // Character with measure length of 4
                else if (character.selectedCharacter.measureLength == 4)
                {
                    if (Metronome.Instance.measureCount % 4 == 0)
                    {
                        character.Play();
                    }
                    else if (Metronome.Instance.measureCount % 4 == 2)
                    {
                        character.PlayAtHalfTime();
                    }
                }
                // Character with measure length of 8
                else if (character.selectedCharacter.measureLength == 8)
                {
                    if (Metronome.Instance.measureCount % 8 == 0)
                    {
                        character.Play();
                    }
                    else if (Metronome.Instance.measureCount % 8 == 4)
                    {
                        character.PlayAtHalfTime();
                    }
                }
            }
        }
    }
    
    public void pause()
    {
        foreach (CharacterObject character in characters)
        {
            character.Stop();
        }
        paused = true;
    }

    public void play()
    {
        paused = false;
    }

    public void ResetAllCharacters()
    {
        StartCoroutine(ResetCharacters());
    }

    // Checks if all characters are ready to play
    private bool IsAllCharactersReady()
    {
        int readyCharacters = 0;

        foreach (CharacterObject character in characters)
        {
            if (character.characterSet)
            {
                readyCharacters++;
                if (!character.readyToPlay)
                {
                    return false;
                }
            }
        }

        if (readyCharacters == 0)
        {
            return false;
        }

        return true;
    }

    // Character animation intro
    IEnumerator CharacterAnimationIntro()
    {
        foreach (CharacterObject character in characters)
        {
            character.Visible(false);
            // Randomize Y position of each character from 0 to randomPosY
            character.transform.position = new Vector3(character.transform.position.x, Random.Range(0, randomPosY), character.transform.position.z);
        }

        foreach (CharacterObject character in characters)
        {
            character.Visible(true);
            character.PlayInto();
            yield return new WaitForSeconds(introTime);
        }
        yield return new WaitForSeconds(1);
    }

    // Resets all characters to default character and randomizes Y position
    IEnumerator ResetCharacters()
    {
        foreach (CharacterObject character in characters)
        {
            character.Reset();
            yield return new WaitForSeconds(introTime);
            character.transform.position = new Vector3(character.transform.position.x, Random.Range(0, randomPosY), character.transform.position.z);
        }
    }
}
