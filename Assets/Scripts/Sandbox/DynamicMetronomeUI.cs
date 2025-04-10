using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicMetronomeUI : MonoBehaviour
{
    public TMP_InputField bpmInputField;
    public DynamicMetronome metronome;

    public void setBPM()
    {
        metronome.SetBPM(float.Parse(bpmInputField.text));
    }
}
