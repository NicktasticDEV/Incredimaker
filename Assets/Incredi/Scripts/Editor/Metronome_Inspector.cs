using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

[CustomEditor(typeof(Metronome))]
public class Metronome_Inspector : Editor
{
    // Adds additional buttons to the Metronome script in the Inspector

    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Metronome metronome = (Metronome)target;

        // Show the count of beats, steps, and measures
        EditorGUILayout.LabelField("Beat Count", metronome.beatCount.ToString());
        EditorGUILayout.LabelField("Step Count", metronome.stepCount.ToString());
        EditorGUILayout.LabelField("Measure Count", metronome.measureCount.ToString());

        // Horizontal layout for Play/Pause button and status label
        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
            {
                metronome.Play();
            }

            if (GUILayout.Button("Pause"))
            {
                metronome.Pause();
            }

            if (GUILayout.Button("Reset"))
            {
                metronome.Reset();
            }
        EditorGUILayout.EndHorizontal();
    }
    

    /*
    public override VisualElement CreateInspectorGUI()
    {
        Metronome metronome = (Metronome)target;
        VisualElement root = new VisualElement();

        // BPM Field
        IntegerField bpmField = new IntegerField("BPM");
        bpmField.value = metronome.BPM;
        bpmField.RegisterValueChangedCallback(evt => metronome.BPM = evt.newValue);
        root.Add(bpmField);

        // Reset button
        Button resetButton = new Button();
        resetButton.text = "Reset";
        resetButton.clicked += metronome.Reset;
        root.Add(resetButton);

        // Beat count
        Label beatCountLabel = new Label();
        root.Add(beatCountLabel);

        // Step count
        Label stepCountLabel = new Label();
        root.Add(stepCountLabel);

        // Measure count
        Label measureCountLabel = new Label();
        root.Add(measureCountLabel);

        // Update the labels
        EditorApplication.update += () =>
        {
            beatCountLabel.text = "Beat Count: " + metronome.beatCount;
            stepCountLabel.text = "Step Count: " + metronome.stepCount;
            measureCountLabel.text = "Measure Count: " + metronome.measureCount;
        };

        return root;
    }
    */

}

