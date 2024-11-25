using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    public List<CharacterObject> characters = new List<CharacterObject>();
    public bool songPlaying = false;
    public bool paused = false;
    public int measureLength = 4;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Subscribe to events
        Metronome.Instance.onMeasure += MeasureHit;
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Metronome.Instance.measureCount > measureLength)
        {
            Metronome.Instance.Reset();
        }

        if (!songPlaying && IsAllCharactersReady() && !paused)
        {
            Metronome.Instance.Reset();
            foreach (CharacterObject character in characters)
            {
                character.Play();
            }
            songPlaying = true;
        }
        
        if (!IsAllCharactersReady())
        {
            songPlaying = false;
        }
    }

    void MeasureHit()
    {
        foreach (CharacterObject character in characters)
        {
            if (character.characterSet)
            {
                if ((Metronome.Instance.measureCount % character.selectedCharacter.measureLength == 0) || (character.selectedCharacter.measureLength == 4 && Metronome.Instance.measureCount == 2 && !character.isPlaying))
                {
                    Debug.Log("h");
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
    }

    public void play()
    {
        paused = false;
    }

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
}
