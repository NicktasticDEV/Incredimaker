using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModManager : MonoBehaviour
{
    public static ModManager Instance;  // Singleton instance
    public List<Mod> mods = new List<Mod>();  // List of mods

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        loadMods();
    }

    void loadMods()
    {
        mods.Clear();

        string path = Application.streamingAssetsPath + "/Mods";

        if (!System.IO.Directory.Exists(path))
        {
            Debug.LogError("Mods directory does not exist.");
            return;
        }

        string[] modFolders = System.IO.Directory.GetDirectories(path);
        Debug.Log("Found mod folders: " + modFolders.Length);

        foreach (string modFolder in modFolders)
        {
            string metadataPath = modFolder + "/metadata.json";

            if (!System.IO.File.Exists(metadataPath))
            {
                Debug.LogError("Metadata file does not exist at: " + metadataPath);
                continue;
            }

            string metadataJson = System.IO.File.ReadAllText(metadataPath);
            //Debug.Log("Metadata JSON: " + metadataJson);

            ModMetadata metadata = JsonUtility.FromJson<ModMetadata>(metadataJson);
            if (metadata == null)
            {
                Debug.LogError("Failed to parse metadata JSON.");
                continue;
            }

            Debug.Log("Mod: " + metadata.modName);
            Mod mod = new Mod(metadata);
            mods.Add(mod);
        }
    }
}
