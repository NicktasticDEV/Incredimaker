using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using Extrite;

public class ExtriteWindow : EditorWindow
{
    [MenuItem("Extrite/Extrite Window")]
    public static void ShowWindow()
    {
        GetWindow<ExtriteWindow>("Extrite Window");
    }

    void OnGUI()
    {
        GUILayout.Label("Extrite Window", EditorStyles.boldLabel);

        // Import Sparrow Animation Pack
        if (GUILayout.Button("Import Sparrow Animation Pack"))
        {
            importAnimationPack();
        }

        // Export Sparrow Animation Pack
        if (GUILayout.Button("Export Sparrow Animation Pack"))
        {
            exportAnimationPack();
        }
    }

    void importAnimationPack()
    {
        Debug.Log("Importing Animation Pack");

        SO_SparrowAnimationPack sparrowAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();

        string sparrowAnimationPackPath = Extrite.Utilities.GetPathFromDialogue("Import Sparrow Animation Pack", "esap", false);

        sparrowAnimationPack = Extrite.Utilities.ImportSparrowAnimationPack(sparrowAnimationPackPath);
        
        // Get texture and atlas from the animation pack
        Texture2D texture = Extrite.Utilities.GetTextureFromAnimationPack(sparrowAnimationPackPath);
        TextAsset atlas = Extrite.Utilities.GetTextAssetFromAnimationPack(sparrowAnimationPackPath);

        // Save the animation pack to the project (Make sure path is relative to the project folder) Use editor utility to save the file
        string path = EditorUtility.SaveFilePanel("Save Animation Pack", Application.dataPath, "New Animation Pack", "asset");
        if (path.Length != 0)
        {
            path = path.Replace(Application.dataPath, "Assets");
            AssetDatabase.CreateAsset(sparrowAnimationPack, path);

            // Put the texture and atlas as sub-assets of the animation pack
            AssetDatabase.AddObjectToAsset(texture, sparrowAnimationPack);
            AssetDatabase.AddObjectToAsset(atlas, sparrowAnimationPack);

            // Make sure the references are set correctly
            sparrowAnimationPack.texture = texture;
            sparrowAnimationPack.atlas = atlas;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log("Imported Animation Pack");
    }

    void exportAnimationPack()
    {
        string animationPackPath = Extrite.Utilities.GetPathFromDialogue("Export Sparrow Animation Pack", "asset", false);

        if (animationPackPath.Length != 0)
        {
            Debug.Log("Selected Animation Pack: " + animationPackPath);
        }
        else
        {
            return;
        }
        
        SO_SparrowAnimationPack sparrowAnimationPack = AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(animationPackPath);

        string exportPath = EditorUtility.SaveFilePanel("Export Animation Pack", "", "animationPack", "esap");
        Extrite.Utilities.ExportSparrowAnimationPack(sparrowAnimationPack, exportPath);

        Debug.Log("Exported Animation Pack");
    }
}
