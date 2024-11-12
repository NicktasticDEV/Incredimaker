using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoirTestManager : MonoBehaviour
{
    public int measureLength = 4;
    private List<TestCharacter> testCharacter;

    void Start()
    {
        Metronome.Instance.onMeasure += onMeasureHit;

        testCharacter = new List<TestCharacter>(FindObjectsOfType<TestCharacter>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Metronome.Instance.measureCount > measureLength)
        {
            Metronome.Instance.Reset();
        }

        // If only one is found ready to play and isn't playing yet, reset metronome
        if (testCharacter.FindAll(c => c.readyToPlay).Count == 1 && !testCharacter.Find(c => c.isPlaying))
        {
            Metronome.Instance.Reset();
        }


    }

    void onMeasureHit()
    {
        Debug.Log($"Measure Hit {Metronome.Instance.measureCount}");
    }
}
