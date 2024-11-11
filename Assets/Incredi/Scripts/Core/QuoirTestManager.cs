using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoirTestManager : MonoBehaviour
{
    public int measureLength = 4;

    void Start()
    {
        Metronome.Instance.onMeasure += onMeasureHit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Metronome.Instance.measureCount > measureLength)
        {
            Metronome.Instance.Reset();
        }
    }

    void onMeasureHit()
    {
        Debug.Log($"Measure Hit {Metronome.Instance.measureCount}");
    }
}
