using UnityEngine;
using System;

public class DynamicMetronome : MonoBehaviour
{
    [Tooltip("BPM for the main beat (can be changed at runtime).")]
    public double bpm = 120.0;

    [Tooltip("Number of main beats per measure.")]
    public int beatsPerMeasure = 4;

    [Tooltip("Number of subbeats per main beat (e.g., 1 = no subdivision, 2 = eighth notes if beat is a quarter note).")]
    public int subBeatsPerBeat = 4;

    [Tooltip("How far ahead (in seconds) beats are scheduled.")]
    public double schedulingAheadTime = 0.1;

    [Tooltip("Determines if the metronome starts paused.")]
    public bool startPaused = true;

    // Subscribable events:
    // OnBeat: first parameter is the current beat index within the measure (0-based); second is the measure count.
    public event Action<int, int> OnBeat;
    // OnSubBeat: parameter is the subbeat index within the current beat (0-based)
    public event Action<int> OnSubBeat;
    // OnMeasure: parameter is the current measure count (0-based)
    public event Action<int> OnMeasure;
    // OnRestart: called when the metronome is restarted.
    public event Action OnRestart;
    // OnPause: called when the metronome is paused.
    public event Action OnPause;
    // OnResume: called when the metronome is resumed.
    public event Action OnResume;
    // OnBPMChanged: called when the BPM is changed.
    public event Action<double> OnBPMChanged;

    // Internal timing and counter state
    private double nextTickTime;   // The dspTime when the next tick should occur.
    private double beatInterval;   // Time between main beats.
    private double subBeatInterval;// Time between subbeats.

    // Counters tracking progress
    private int currentBeatInMeasure = 0; // 0 to (beatsPerMeasure - 1)
    private int currentSubBeatInBeat = 0;   // 0 to (subBeatsPerBeat - 1)
    private int measureCount = 0;

    // Pause/resume state
    public bool isPaused { get; private set; } = true;
    private double pauseStartTime = 0.0;

    // MISC
    private bool hasStarted = false; // Flag to check if the metronome has started playing.

    void Start()
    {
        UpdateIntervals();

        // Set the paused state based on the inspector setting.
        isPaused = startPaused;

        // Only schedule the first tick if not starting paused.
        if (!isPaused)
        {
            nextTickTime = AudioSettings.dspTime + schedulingAheadTime;
            ScheduleTick(); // Kick off the first tick.
            hasStarted = true; // Set the flag to indicate that the metronome has started playing.
        }
    }

    /// <summary>
    /// Recalculate beat/subbeat intervals based on the current BPM.
    /// </summary>
    void UpdateIntervals()
    {
        // Time between main beats.
        beatInterval = 60.0 / bpm;
        // Divide the beat into subbeats.
        subBeatInterval = beatInterval / subBeatsPerBeat;
    }

    /// <summary>
    /// Schedules a single tick and fires the relevant events.
    /// This method assumes that each tick occurs at subBeatInterval intervals.
    /// </summary>
    void ScheduleTick()
    {
        // Notify subscribers for every subbeat.
        OnSubBeat?.Invoke(currentSubBeatInBeat);
        
        // If this tick is the first subbeat of a main beat, fire the OnBeat event.
        if (currentSubBeatInBeat == 0)
        {
            OnBeat?.Invoke(currentBeatInMeasure, measureCount);
            // If this is also the first beat of a measure, fire OnMeasure.
            if (currentBeatInMeasure == 0)
                OnMeasure?.Invoke(measureCount);
        }

        // Advance the counters.
        currentSubBeatInBeat++;
        if (currentSubBeatInBeat >= subBeatsPerBeat)
        {
            currentSubBeatInBeat = 0;
            currentBeatInMeasure++;
            if (currentBeatInMeasure >= beatsPerMeasure)
            {
                currentBeatInMeasure = 0;
                measureCount++;
            }
        }

        // Increment nextTickTime by the subbeat interval.
        nextTickTime += subBeatInterval;
    }

    void Update()
    {
        // Do not process scheduling if paused.
        if (isPaused) return;

        // Ensure intervals are updated (this also makes dynamic BPM changes effective).
        UpdateIntervals();

        double dspTime = AudioSettings.dspTime;
        // Schedule ticks as long as we are within the scheduling window.
        while (dspTime + schedulingAheadTime >= nextTickTime)
        {
            ScheduleTick();
        }
    }

    /// <summary>
    /// Dynamically changes the BPM. This recalculates the timing intervals.
    /// </summary>
    public void SetBPM(double newBpm)
    {
        bpm = newBpm;
        UpdateIntervals();
        // Notify subscribers of the BPM change.
        OnBPMChanged?.Invoke(bpm);
    }

    /// <summary>
    /// Pauses the metronome.
    /// </summary>
    public void Pause()
    {
        if (isPaused) return;
        // Record the dspTime when pause is called.
        pauseStartTime = AudioSettings.dspTime;
        isPaused = true;
        // Notify subscribers of the pause event.
        OnPause?.Invoke();
    }

    /// <summary>
    /// Resumes the metronome from pause.
    /// </summary>
    public void Resume()
    {
        if (!isPaused) return;

        // This is the best band-aid solution to it to avoid the weird 1st beat and 2nd beat being too close. Maybe can be improved later.
        if (hasStarted)
        {
            // Calculate the duration of the pause and shift nextTickTime forward by that duration.
            double pausedDuration = AudioSettings.dspTime - pauseStartTime;
            nextTickTime += pausedDuration;
            isPaused = false;
            OnResume?.Invoke();
        }
        else
        {
            // Reset the next tick time to a short delay in the future.
            nextTickTime = AudioSettings.dspTime + schedulingAheadTime;
            isPaused = false;
            OnResume?.Invoke();
            hasStarted = true; // Set the flag to indicate that the metronome has started playing.
        }
    }

    /// <summary>
    /// Restarts the metronome: resets counters and timing.
    /// </summary>
    public void Restart()
    {
        // Reset all counters.
        currentBeatInMeasure = 0;
        currentSubBeatInBeat = 0;
        measureCount = 0;
        // Reset the next tick time to a short delay in the future.
        nextTickTime = AudioSettings.dspTime + schedulingAheadTime;
        // Ensure the metronome is not paused.
        isPaused = false;
        // Notify subscribers of the restart event.
        OnRestart?.Invoke();
        hasStarted = true; // Set the flag to indicate that the metronome has started playing.
    }
}