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
            //Load Mod Metadata
            string metadataPath = modFolder + "/metadata.json";

            if (!System.IO.File.Exists(metadataPath))
            {
                Debug.LogError("Metadata file does not exist at: " + metadataPath);
                continue;
            }

            string metadataJson = System.IO.File.ReadAllText(metadataPath);
            //Debug.Log("Metadata JSON: " + metadataJson);

            Mod mod = JsonUtility.FromJson<Mod>(metadataJson);
            if (mod == null)
            {
                Debug.LogError("Failed to parse metadata JSON.");
                continue;
            }

            Debug.Log("Loaded mod: " + mod.modName);

            //Load Mod Characters

            string charactersPath = modFolder + "/assets/characters";
            if (!System.IO.Directory.Exists(charactersPath))
            {
                Debug.LogError("Characters directory does not exist at: " + charactersPath);
                continue;
            }

            string[] characterFolders = System.IO.Directory.GetDirectories(charactersPath);

            List<Character> characters = new List<Character>();

            foreach (string characterFolder in characterFolders)
            {
                string characterMetadataPath = characterFolder + "/metadata.json";

                if (!System.IO.File.Exists(characterMetadataPath))
                {
                    Debug.LogError("Character metadata file does not exist at: " + characterMetadataPath);
                    continue;
                }

                string characterMetadataJson = System.IO.File.ReadAllText(characterMetadataPath);
                //Debug.Log("Character Metadata JSON: " + characterMetadataJson);

                Character characterMetadata = JsonUtility.FromJson<Character>(characterMetadataJson);
                if (characterMetadata == null)
                {
                    Debug.LogError("Failed to parse character metadata JSON.");
                    continue;
                }

                //Load Thumbnail
                string thumbnailPath = characterFolder + "/thumbnail.png";

                if (!System.IO.File.Exists(thumbnailPath))
                {
                    Debug.LogError("Thumbnail file does not exist at: " + thumbnailPath);
                    continue;
                }

                byte[] thumbnailBytes = System.IO.File.ReadAllBytes(thumbnailPath);

                Texture2D thumbnail = new Texture2D(512, 512);
                thumbnail.LoadImage(thumbnailBytes);

                characterMetadata.thumbnail = thumbnail;

                //Load Voice
                string voicePath = characterFolder + "/vocals.wav";

                if (!System.IO.File.Exists(voicePath))
                {
                    Debug.LogError("Voice file does not exist at: " + voicePath);
                    continue;
                }

                AudioClip voice = LoadExternalAudio.LoadAudioClip(voicePath);
                characterMetadata.voice = voice;

                characters.Add(characterMetadata);
            }

            mod.characters = characters;
            mods.Add(mod);
        }
    }


}
