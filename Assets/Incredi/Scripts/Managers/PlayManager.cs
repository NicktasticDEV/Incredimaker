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

    [SerializeField]
    private float randomPosY = 0;

    public bool iconBeingDragged = false;
    public string selectedCharacter = "";

    [SerializeField]
    private float introTime = 1.5f;
    
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
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        Metronome.Instance.onMeasure += MeasureHit;
        Metronome.Instance.onMeasureLate += MeasureHitLate;

        Metronome.Instance.BPM = ModManager.Instance.mods[0].songBPM;
        Metronome.Instance.defaultBPM = ModManager.Instance.mods[0].songBPM;

        foreach (CharacterObject character in characters)
        {
            character.SetCharacter(ModManager.Instance.GetCharacter("Default"));
        }

        StartCoroutine(Intro());
    }

    [ContextMenu("Reset All Characters")]
    public void ResetAllCharacters()
    {
        StartCoroutine(Reset());
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

    void MeasureHitLate()
    {

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

    IEnumerator Intro()
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

    IEnumerator Reset()
    {
        foreach (CharacterObject character in characters)
        {
            character.Reset();
            yield return new WaitForSeconds(introTime);
            character.transform.position = new Vector3(character.transform.position.x, Random.Range(0, randomPosY), character.transform.position.z);
        }
    }
}
