using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class Metronome : MonoBehaviour
{
    public static Metronome Instance { get; private set; }  // Singleton instance

    public int BPM = 120;                   // Beats per minute
    public bool playSound = true;           // Play metronome sound
    public bool isPlaying = false;          // Controls whether the metronome is playing
    private float beatInterval;              // Time between beats in seconds
    private float nextBeatTime;              // Tracks the next beat time
    private List<AudioSource> bpmAudioSources = new List<AudioSource>();
    private const int defaultBPM = 120;      // Default BPM of audio sources

    // Sounds
    [SerializeField] private AudioClip beatSound;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip measureSound;

    // Events
    public event Action onBeat;
    public event Action onStep;
    public event Action onMeasure;

    public int beatCount = 0;            // Counts the number of beats
    public int stepCount = 0;            // Counts the steps
    public int measureCount = 0;         // Counts the measures
    public int beatsPerMeasure = 4;       // Number of beats in one measure
    public int stepsPerBeat = 1;          // Number of steps per beat
    private float stepInterval;           // Time between steps
    private double nextStepTime;           // Tracks the next step time

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    void Start()
    {
        UpdateInterval();
        FindBPMAudioSources();
        UpdateAudioSourcesPitch();
        stepInterval = beatInterval / stepsPerBeat;
        nextStepTime = AudioSettings.dspTime;
    }

    void Update()
    {
        if (!isPlaying) return;  // Stop if not playing

        double time = AudioSettings.dspTime;
        if (time >= nextStepTime)
        {
            PlayStep();
            nextStepTime += stepInterval;
        }
    }

    private void UpdateInterval()
    {
        beatInterval = 60f / BPM;
    }

    private void PlayStep()
    {
        onStep?.Invoke();  // Trigger onStep event
        stepCount++;

        if (playSound && stepSound != null)
        {
            AudioSource.PlayClipAtPoint(stepSound, transform.position);
        }

        if (stepCount % stepsPerBeat == 1)
        {
            onBeat?.Invoke();     // Trigger onBeat event
            beatCount++;

            if (playSound && beatSound != null)
            {
                AudioSource.PlayClipAtPoint(beatSound, transform.position);
            }

            if (beatCount % beatsPerMeasure == 1)
            {
                onMeasure?.Invoke();  // Trigger onMeasure event
                measureCount++;

                if (playSound && measureSound != null)
                {
                    AudioSource.PlayClipAtPoint(measureSound, transform.position);
                }
            }
        }
    }

    private void FindBPMAudioSources()
    {
        GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("BPMAudio");
        foreach (GameObject obj in audioObjects)
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source != null)
            {
                bpmAudioSources.Add(source);
            }
        }
    }

    private void UpdateAudioSourcesPitch()
    {
        float pitchFactor = (float)BPM / defaultBPM;
        foreach (AudioSource source in bpmAudioSources)
        {
            source.pitch = pitchFactor;  // Adjust pitch to match current BPM
        }
    }

    public void Reset()
    {
        isPlaying = false;  // Stop the metronome
        beatCount = 0;
        stepCount = 0;
        measureCount = 0;
        nextStepTime = AudioSettings.dspTime;  // Reset the step time
    }
}
