using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Metronome.Instance.onBeat += OnBeat;
        Metronome.Instance.onStep += OnStep;
        Metronome.Instance.onMeasure += OnMeasure;
    }

    void OnBeat()
    {
        Debug.Log("Beat!");
    }

    void OnStep()
    {
        Debug.Log("Step!");
    }

    void OnMeasure()
    {
        Debug.Log("Measure!");
    }
}
