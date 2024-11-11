using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    // Dropdown textmeshpro
    [SerializeField] private TMP_Dropdown bpmModeSelector;

    // Fields
    [SerializeField] private TMP_InputField bpmField;

    // Buttons
    [SerializeField] private Button changeBpmButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;


    void Awake()
    {
        bpmModeSelector.onValueChanged.AddListener(OnBpmModeSelectorChanged);
        changeBpmButton.onClick.AddListener(ChangeBpm);
        resetButton.onClick.AddListener(() => Metronome.Instance.Reset());
        playButton.onClick.AddListener(() => Metronome.Instance.Play());
        pauseButton.onClick.AddListener(() => Metronome.Instance.Pause());
    }

    void Start()
    {
        // Set the default values
        bpmField.text = Metronome.Instance.BPM.ToString();
    }

    private void OnBpmModeSelectorChanged(int value)
    {
        string selectedBpmMode = bpmModeSelector.options[value].text;
        Debug.Log("Selected BPM Mode: " + selectedBpmMode);
    }

    public void ChangeBpm()
    {
        int newBpm = int.Parse(bpmField.text);
        Metronome.Instance.BPM = newBpm;
        Debug.Log("New BPM: " + newBpm);
    }
}
