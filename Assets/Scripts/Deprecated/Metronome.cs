using UnityEngine;
using System;

namespace Incredimaker
{
    public class Metronome : MonoBehaviour
    {
        public static Metronome Instance { get; private set; }  // Singleton instance

        [Header("Metronome Settings")]
        public int BPM = 120;                 // Number of Beats per minute
        public int stepsPerBeat = 4;          // Number of steps per beat
        public int beatsPerMeasure = 4;       // Number of beats per measure
        [SerializeField] private bool playOnStart = true; // Play metronome on start

        [Header("Sound Settings")]
        [SerializeField] private bool playSound = true; // Play metronome sound (useful for testing)
        [SerializeField] private AudioClip measureSound;
        [SerializeField] private AudioClip beatSound;
        [SerializeField] private AudioClip stepSound;

        // Properties
        [HideInInspector] public float beatInterval { get; private set; } // Time between beats in seconds
        [HideInInspector] public int previousBPM { get; private set; }
        [HideInInspector] public bool isActive { get; private set; }      // Indicates if the metronome is active
        [HideInInspector] public bool isPaused { get; private set; } = false; // Indicates if the metronome is paused

        // Events
        public event Action onBeat;
        public event Action onStep;
        public event Action onMeasure;
        public event Action<int> onBPMChange;
        public event Action onReset;
        public event Action onPlay;
        public event Action onPause;
        
        // Private variables
        private float stepInterval;           // Time between steps
        private double nextStepTime;          // Tracks the next step time
        private int beatCount = 0;            // Counts the number of beats
        private int stepCount = 0;            // Counts the steps
        private int measureCount = 0;         // Counts the measures

        void Awake()
        {
            // Set singleton instance
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
            stepInterval = beatInterval / stepsPerBeat;
            nextStepTime = AudioSettings.dspTime;
            previousBPM = BPM;

            if (playOnStart)
            {
                Play();
            }
        }

        void Update()
        {
            // Check if metronome is playing
            if (!isActive) return;
            if (isPaused) return;

            if (BPM != previousBPM)
            {
                UpdateInterval();
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
            stepCount++;
            onStep?.Invoke();  // Trigger onStep event

            if (playSound && stepSound != null)
            {
                AudioSource.PlayClipAtPoint(stepSound, transform.position);
            }

            if (stepCount % stepsPerBeat == 1)
            {
                beatCount++;
                onBeat?.Invoke();     // Trigger onBeat event

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

        public void Play()
        {
            isPaused = false;
            isActive = true;
            nextStepTime = AudioSettings.dspTime;
            onPlay?.Invoke();
        }

        public void Pause()
        {
            isPaused = true;
            onPause?.Invoke();
        }
        
    }
}