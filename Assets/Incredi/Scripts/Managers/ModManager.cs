using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extrite;

public class ModManager : MonoBehaviour
{
    public static ModManager Instance;  // Singleton instance

    // Paths
    string modsPath;
    
    // List of mods
    public List<Mod> mods = new List<Mod>();

    // Settings
    [Header("Settings")]
    [SerializeField]
    private bool preloadAllAnimations = true;

    void Awake()
    {
        modsPath = Application.persistentDataPath + "/Mods";

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }

        loadSpecificMod("ModExample");
    }

    void loadSpecificMod(string folderName)
    {
        // Paths
        string metadataPath = modsPath + "/" + folderName + "/metadata.json";
        string charactersPath = modsPath + "/" + folderName + "/assets/characters";

        // Check if metadata exists
        if (!System.IO.File.Exists(metadataPath))
        {
            Debug.LogError("Metadata file does not exist at: " + metadataPath);
            return;
        }

        // Check if characters directory exists
        if (!System.IO.Directory.Exists(charactersPath))
        {
            Debug.LogError("Characters directory does not exist at: " + charactersPath);
            return;
        }

        // Load Mod Metadata
        string metadataJson = System.IO.File.ReadAllText(metadataPath);
        Mod mod = JsonUtility.FromJson<Mod>(metadataJson);
        if (mod == null)
        {
            Debug.LogError("Failed to parse metadata JSON.");
            return;
        }
        Debug.Log("Mod detected: " + mod.modName);

        // Load all character folders
        string[] characterFolders = System.IO.Directory.GetDirectories(charactersPath);
        List<Character> characters = new List<Character>();

        // Load character settings
        string characterSettingsPath = charactersPath + "/settings.json";
        if (System.IO.File.Exists(characterSettingsPath))
        {
            string characterSettingsJson = System.IO.File.ReadAllText(characterSettingsPath);
            CharacterSettings characterSettings = JsonUtility.FromJson<CharacterSettings>(characterSettingsJson);
            mod.characterSettings = characterSettings;
        }

        foreach (string characterFolder in characterFolders)
        {
            // Paths
            string characterMetadataPath = characterFolder + "/metadata.json";
            //string characterAnimationPath = characterFolder + "/animations";
            string characterAnimationPath = characterFolder + "/animations.esap";
            string thumbnailPath = characterFolder + "/thumbnail.png";
            //string animationMetadataPath = characterAnimationPath + "/animations.json";

            // Check if character metadata exists
            if (!System.IO.File.Exists(characterMetadataPath))
            {
                Debug.LogError("Character metadata file does not exist at: " + characterMetadataPath);
                continue;
            }

            // Check if character animations exist
            if (!System.IO.File.Exists(characterAnimationPath))
            {
                Debug.LogError("Character animations directory does not exist at: " + characterAnimationPath);
                continue;
            }

            // Check if thumbnail exists
            if (!System.IO.File.Exists(thumbnailPath))
            {
                Debug.LogError("Thumbnail file does not exist at: " + thumbnailPath);
                continue;
            }

            /*
            // Check if animation metadata exists
            if (!System.IO.File.Exists(animationMetadataPath))
            {
                Debug.LogError("Animation metadata file does not exist at: " + animationMetadataPath);
                continue;
            }
            */

            // Load animations
            

            // Load Character Metadata
            string characterMetadataJson = System.IO.File.ReadAllText(characterMetadataPath);
            Character characterMetadata = JsonUtility.FromJson<Character>(characterMetadataJson);
            
            if (characterMetadata == null)
            {
                Debug.LogError("Failed to parse character metadata JSON.");
                continue;
            }

            //Load Thumbnail
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

            /*
            // Load animations
            string animationMetadataJson = System.IO.File.ReadAllText(animationMetadataPath);
            ImageSequenceAnimations animations = JsonUtility.FromJson<ImageSequenceAnimations>(animationMetadataJson);
            if (animations == null)
            {
                Debug.LogError("Failed to parse animation metadata JSON.");
                continue;
            }

            foreach (ImageSequenceAnimation animation in animations.animations)
            {
                animation.frames = ImageSequenceAnimation.loadFrames(characterAnimationPath + "/" + animation.path);
            }

            characterMetadata.animations = animations;
            */

            // Load Extrite Animations
            characterMetadata.extriteAnimations = Extrite.Utilities.ImportSparrowAnimationPack(characterAnimationPath);

            if (preloadAllAnimations)
            {
                Extrite.SparrowRenderer.PreloadSpecificAnimationPack(characterMetadata.extriteAnimations);
            }

            characters.Add(characterMetadata);

            Debug.Log("Character loaded: " + characterMetadata.name);
        }

        mod.characters = characters;
        mods.Add(mod);

        Debug.Log("Mod loaded: " + mod.modName);
    }

    void unloadAllMods()
    {
        mods.Clear();
    }

    // Get characters from specific mod function
    public List<Character> GetCharactersFromModName(string modName)
    {
        foreach (Mod mod in mods)
        {
            if (mod.modName == modName)
            {
                return mod.characters;
            }
        }

        return null;
    }

    public List<Character> GetCharactersFromModIndex(int modIndex)
    {
        if (modIndex < mods.Count)
        {
            return mods[modIndex].characters;
        }

        return null;
    }

    public Character GetCharacter(string characterName)
    {
        foreach (Mod mod in mods)
        {
            foreach (Character character in mod.characters)
            {
                if (character.name == characterName)
                {
                    return character;
                }
            }
        }

        return null;
    }
}
