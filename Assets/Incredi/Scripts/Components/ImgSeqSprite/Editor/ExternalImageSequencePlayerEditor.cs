using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(ExternalImageSequencePlayer))]
public class ExternalImageSequencePlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExternalImageSequencePlayer sequencePlayer = (ExternalImageSequencePlayer)target;

        // Horizontal layout for Play/Pause button and status label
        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
            {
                sequencePlayer.Play();
            }

            if (GUILayout.Button("Pause"))
            {
                sequencePlayer.Pause();
            }

            if (GUILayout.Button("Reset"))
            {
                sequencePlayer.Reset();
            }
        EditorGUILayout.EndHorizontal();
    }
}
