using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TestCharacter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage characterImage;
    [SerializeField] private TMP_Dropdown characterDropdown;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image timeTilPlayRadialProgress;
    private AudioClip vocalClip;

    public bool readyToPlay = false;
    public bool isPlaying = false;

    private List<Character> characters = new List<Character>();
    private Character selectedCharacter;

    void Awake()
    {
        characterDropdown.onValueChanged.AddListener(characterDropdownValueChanged); 
    }

    // Start is called before the first frame update
    void Start()
    {
        Metronome.Instance.onMeasure += onMeasureHit;

        characters = ModManager.Instance.GetCharactersFromModName("Mod Example");

        characterDropdown.options.Clear();
        characterDropdown.options.Add(new TMP_Dropdown.OptionData("None"));

        foreach (Character character in characters)
        {
            characterDropdown.options.Add(new TMP_Dropdown.OptionData(character.name));
        }
    }

    void ResetCharacterSelection()
    {
        nameText.text = "";
        characterImage.texture = null;
        readyToPlay = false;
        isPlaying = false;
        audioSource.Stop();
    }

    void characterDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            ResetCharacterSelection();
            return;
        }

        selectedCharacter = characters[index - 1];
        nameText.text = selectedCharacter.name;
        characterImage.texture = selectedCharacter.thumbnail;
        audioSource.clip = selectedCharacter.voice;
        readyToPlay = true;
    }

    void onMeasureHit()
    {
        if (!readyToPlay)
        {
            return;
        }

        if ((Metronome.Instance.measureCount % selectedCharacter.measureLength == 0) || (selectedCharacter.measureLength == 4 && Metronome.Instance.measureCount == 2 && !isPlaying))
        {
            audioSource.time = (selectedCharacter.measureLength == 4 && Metronome.Instance.measureCount == 2 && !isPlaying) ? audioSource.clip.length / 2 : 0;
            isPlaying = true;
            audioSource.Play();
        }
    }

}
