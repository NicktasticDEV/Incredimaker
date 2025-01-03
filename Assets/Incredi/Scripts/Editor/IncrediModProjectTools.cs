using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class IncredimodProjectTools : EditorWindow
{
    [MenuItem("Incredimaker/Open Persistent Data Folder")]
    public static void OpenPersistentDataFolder()
    {
        string path = Application.persistentDataPath;
        
        if (Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Debug.LogError("Path does not exist: " + path);
        }
        
    }
}
