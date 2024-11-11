using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeViz : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Metronome.Instance.onBPMChange += OnBPMChange;
        Metronome.Instance.onReset += OnReset;
        Metronome.Instance.onPlay += onPlay;
        Metronome.Instance.onPause += onPause;

        float speed = Metronome.Instance.BPM / 120f;
        animator.speed = speed;
    }

    private void OnBPMChange(int bpm)
    {
        float speed = bpm / 120f;
        animator.speed = speed;
    }

    private void OnReset()
    {
        // restart the animation
        animator.Play("Metronome", 0, 0);
    }

    private void onPause()
    {
        animator.speed = 0;
    }

    private void onPlay()
    {
        float speed = Metronome.Instance.BPM / 120f;
        animator.speed = speed;
    }
}
