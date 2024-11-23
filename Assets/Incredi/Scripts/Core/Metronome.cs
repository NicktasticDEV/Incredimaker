using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class Metronome : MonoBehaviour
{
    public static Metronome Instance { get; private set; }  // Singleton instance

    public int BPM = 120;                   // Beats per minute
    public int defaultBPM = 120;      // Default BPM of audio sources
    public bool playSound = true;           // Play metronome sound
    [HideInInspector] public float beatInterval;              // Time between beats in seconds
    private List<AudioSource> bpmAudioSources = new List<AudioSource>();
    private List<AudioSource> bpmAudioSourcesUnmanaged = new List<AudioSource>();
    private int previousBPM;
    public bool isPlaying = true;  // Track play state

    // Sounds
    [SerializeField] private AudioClip beatSound;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip measureSound;

    // Events
    public event Action onBeat;
    public event Action onStep;
    public event Action onMeasure;
    public event Action<int> onBPMChange;
    public event Action onReset;
    public event Action onPlay;
    public event Action onPause;

    [HideInInspector]
    public int beatCount = 0;            // Counts the number of beats
    [HideInInspector]
    public int stepCount = 0;            // Counts the steps
    [HideInInspector]
    public int measureCount = 0;         // Counts the measures
    
    [HideInInspector] 
    public int beatsPerMeasure = 4;       // Number of beats in one measure
    private int stepsPerBeat = 4;          // Number of steps per beat
    private float stepInterval;           // Time between steps
    private double nextStepTime;           // Tracks the next step time

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        previousBPM = BPM;
    }

    void Update()
    {
        if (!isPlaying) return;  // Check if metronome is playing

        if (BPM != previousBPM)
        {
            UpdateInterval();
            UpdateAudioSourcesPitch();
            stepInterval = beatInterval / stepsPerBeat;

            // Adjust nextStepTime to maintain sync without resetting
            double timeSinceLastStep = AudioSettings.dspTime - (nextStepTime - stepInterval);
            double stepProgress = timeSinceLastStep / stepInterval;
            nextStepTime = AudioSettings.dspTime + (stepInterval - (stepProgress * stepInterval));

            previousBPM = BPM;

            onBPMChange?.Invoke(BPM);
        }
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

        GameObject[] audioObjectsUnmanaged = GameObject.FindGameObjectsWithTag("BPMAudioUnmanaged");
        foreach (GameObject obj in audioObjectsUnmanaged)
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source != null)
            {
                bpmAudioSourcesUnmanaged.Add(source);
            }
        }
    }

    private void UpdateAudioSourcesPitch()
    {
        float newPitch = (float)BPM / defaultBPM;
        foreach (AudioSource source in bpmAudioSources)
        {
            source.pitch = newPitch;  // Adjust pitch to match current BPM
        }

        foreach (AudioSource source in bpmAudioSourcesUnmanaged)
        {
            source.pitch = newPitch;  // Adjust pitch to match current BPM
        }
    }

    public void Play()
    {
        isPlaying = true;
        //nextStepTime = AudioSettings.dspTime + stepInterval;
        nextStepTime = AudioSettings.dspTime;

        onPlay?.Invoke();

        // Play all BPM audio sources
        foreach (AudioSource source in bpmAudioSources)
        {
            //source.Play();
            source.PlayScheduled(AudioSettings.dspTime);
        }
    }

    public void Pause()
    {
        isPlaying = false;
        onPause?.Invoke();

        // Pause all BPM audio sources
        foreach (AudioSource source in bpmAudioSources)
        {
            source.Pause();
        }
    }

    [ContextMenu("Reset Metronome")]
    public void Reset()
    {
        UpdateInterval();
        UpdateAudioSourcesPitch();
        stepInterval = beatInterval / stepsPerBeat;
        nextStepTime = AudioSettings.dspTime;
        beatCount = 0;
        stepCount = 0;
        measureCount = 0;
        foreach (AudioSource source in bpmAudioSources)
        {
            source.time = 0;
        }

        onReset?.Invoke();
    }
    
}
