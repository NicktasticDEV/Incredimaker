using System.Collections;
using System.Collections.Generic;
using Incredimaker;
using UnityEngine;

public class MetronomeTestManager : MonoBehaviour
{
    public static MetronomeTestManager instance;
    public TMPro.TMP_InputField bpmInputField;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayMetronome()
    {
        Metronome.Instance.Play();
    }

    public void ChangeBPM()
    {
        Metronome.Instance.BPM = int.Parse(bpmInputField.text);
    }
}
