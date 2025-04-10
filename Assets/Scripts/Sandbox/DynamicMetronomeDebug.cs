using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMetronomeDebug : MonoBehaviour
{
    private DynamicMetronome metronome;
    private AudioSource audioSource;

    public AudioClip beatClip;
    public AudioClip subBeatClip;
    public AudioClip measureClip;
    public AudioClip musicClip;

    private bool hasStarted = false;

    void Awake()
    {
        metronome = GetComponent<DynamicMetronome>();
        audioSource = GetComponent<AudioSource>();

        if (metronome == null)
        {
            Debug.LogError("DynamicMetronome component not found on this GameObject.");
            return;
        }

        metronome.OnBeat += onBeat;
        metronome.OnSubBeat += onSubBeat;
        metronome.OnMeasure += onMeasure;
        metronome.OnPause += onPause;
        metronome.OnResume += onResume;
        metronome.OnRestart += OnRestart;
        metronome.OnBPMChanged += OnBPMChanged;
    }

    void onBeat(int beatIndex, int measureCount)
    {
        if (beatClip != null)
        {
            AudioSource.PlayClipAtPoint(beatClip, Camera.main.transform.position);
        }

        if (!hasStarted && musicClip != null)
        {
            audioSource.Play();
        }
        hasStarted = true;
    }

    void onSubBeat(int subBeatIndex)
    {
        if (subBeatClip != null)
        {
            AudioSource.PlayClipAtPoint(subBeatClip, Camera.main.transform.position);
        }
    }

    void onMeasure(int measureCount)
    {
        if (measureClip != null)
        {
            AudioSource.PlayClipAtPoint(measureClip, Camera.main.transform.position);
        }
    }

    void onPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    void onResume()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    void OnRestart()
    {
        if (musicClip != null)
        {
            audioSource.time = 0f; // Reset the audio source time to the beginning
            audioSource.Play();
        }
    }

    void OnBPMChanged(double bpm)
    {
        // The song's BPM is 120, so we need to adjust the audio speed accordingly.
        audioSource.pitch = (float)(bpm / 120.0);
    }

}
