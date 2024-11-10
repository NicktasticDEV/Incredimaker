using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    public static Metronome Instance { get; private set; }  // Singleton instance

    public int BPM = 120;                   // Beats per minute
    private float beatInterval;              // Time between beats in seconds
    private float nextBeatTime;              // Tracks the next beat time
    private AudioSource audioSource;         // Main metronome tick audio source
    private List<AudioSource> bpmAudioSources = new List<AudioSource>();
    private const int defaultBPM = 120;      // Default BPM of audio sources

    // Events
    public event Action onBeat;
    public event Action onStep;
    public event Action onMeasure;

    private int beatCount = 0;            // Counts the number of beats
    public int beatsPerMeasure = 4;       // Number of beats in one measure
    public int stepsPerBeat = 1;          // Number of steps per beat
    private float stepInterval;           // Time between steps
    private float nextStepTime;           // Tracks the next step time
    private int stepCount = 0;            // Counts the steps

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
        audioSource = GetComponent<AudioSource>();
        UpdateInterval();
        FindBPMAudioSources();              // Finds all audio sources with "BPMAudio" tag
        UpdateAudioSourcesPitch();           // Adjusts pitch to match current BPM
        nextBeatTime = Time.time;
        stepInterval = beatInterval / stepsPerBeat;
        nextStepTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextStepTime)
        {
            PlayStep();
            nextStepTime += stepInterval;    // Schedules the next step
        }
    }

    public void SetBPM(int newBPM)
    {
        BPM = newBPM;
        UpdateInterval();
        UpdateAudioSourcesPitch();           // Updates pitch for new BPM
    }

    private void UpdateInterval()
    {
        beatInterval = 60f / BPM;
    }

    private void PlayTick()
    {
        audioSource.Play();
    }

    private void PlayStep()
    {
        onStep?.Invoke();  // Trigger onStep event
        stepCount++;

        if (stepCount % stepsPerBeat == 0)
        {
            PlayTick();
            onBeat?.Invoke();     // Trigger onBeat event
            beatCount++;

            if (beatCount % beatsPerMeasure == 0)
            {
                onMeasure?.Invoke();  // Trigger onMeasure event
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
}
