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
            if (character.characterSet)
            {
                if ((Metronome.Instance.measureCount % character.selectedCharacter.measureLength == 0) || (character.selectedCharacter.measureLength == 4 && Metronome.Instance.measureCount == 2 && !character.isPlaying))
                {
                    if (character.selectedCharacter.measureLength == 4 && Metronome.Instance.measureCount == 2 && character.readyToPlay)
                    {
                        character.PlayAtHalfTime();
                        Debug.Log("Half time");
                    }
                    else
                    {
                        character.Play();
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
        }
    }
}
