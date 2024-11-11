using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterPreviewUI : MonoBehaviour
{

    // Lists
    [SerializeField] private TMP_Dropdown modSelector;
    [SerializeField] private TMP_Dropdown characterSelector;

    // Buttons
    [SerializeField] private Button modSelectButton;
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;

    // Fields
    [SerializeField] private RawImage characterThumbnail;
    [SerializeField] private TMP_Text characterName;

    //Misc
    [SerializeField] private AudioSource audioSource;


    private Mod selectedMod;
    private Character selectedCharacter;

    private bool isPlaying = false;

    void Awake()
    {
        modSelectButton.onClick.AddListener(() =>
        {
            modSelected(modSelector.value);
        });

        characterSelectButton.onClick.AddListener(() =>
        {
            characterSelected(characterSelector.value);
        });

        playButton.onClick.AddListener(play);
        pauseButton.onClick.AddListener(pause);
    }

    // Start is called before the first frame update
    void Start()
    {
        Metronome.Instance.onMeasure += onMeasureHit;

        modSelector.ClearOptions();

        List<string> modNames = new List<string>();
        foreach (Mod mod in ModManager.Instance.mods)
        {
            modNames.Add(mod.modName);
        }
        modSelector.AddOptions(modNames);
    }

    private void modSelected(int index)
    {
        selectedMod = ModManager.Instance.mods[index];

        characterSelector.ClearOptions();

        List<string> characterNames = new List<string>();
        foreach (Character character in selectedMod.characters)
        {
            characterNames.Add(character.name);
        }
        characterSelector.AddOptions(characterNames);

        Metronome.Instance.BPM = selectedMod.songBPM;
        Metronome.Instance.Reset();

        Debug.Log("Selected mod: " + selectedMod.modName);
    }

    private void characterSelected(int index)
    {
        selectedCharacter = selectedMod.characters[index];

        characterThumbnail.texture = selectedCharacter.thumbnail;
        characterName.text = selectedCharacter.name;

        audioSource.clip = selectedCharacter.voice;

        Debug.Log("Selected character: " + selectedCharacter.name);
    }

    private void play()
    {
        Metronome.Instance.Reset();

        audioSource.time = 0;
        audioSource.Play();

        isPlaying = true;
    }

    private void pause()
    {
        Metronome.Instance.Pause();
        audioSource.Pause();

        isPlaying = false;
    }

    private void onMeasureHit()
    {
        if (!isPlaying)
        {
            return;
        }
        
        if (Metronome.Instance.measureCount % selectedCharacter.measureLength == 0)
        {
            audioSource.time = 0;
            audioSource.Play();
        }
    }

}
