using System;
using Extrite;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtriteEditorManager : MonoBehaviour
{
    [Header("Files")]
    public SO_SparrowAnimationPack loadedSparrowAnimationPack;
    public SO_SparrowAnimationPack editingSparrowsAnimationPack;

    [Header("Renderers")]
    public SparrowRenderer mainSparrowRenderer;
    public SparrowRenderer ghostSparrowRenderer;

    [Header("File Management UI")]
    public Toggle ghostSparrowToggle;
    [Header("Animation Management UI")]
    public TMP_Dropdown animationDropdown;
    public TMP_InputField animationNameInputField;
    public TMP_InputField prefixInputField;
    public TMP_InputField fpsInputField;
    public Toggle loopToggle;


    [Header("Misc")]
    public bool currentlyWritingFile = false;

    void Awake()
    {
        editingSparrowsAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();

        // Events
        ghostSparrowToggle.onValueChanged.AddListener(toggleGhostSparrow);

        animationDropdown.onValueChanged.AddListener(ChangedSelectedAnimation);
    }

    

    #region Animation Pack File Management
    // New Animation Pack
    public void NewAnimationPack()
    {
        editingSparrowsAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();
        UpdateAnimationList();
    }

    // Load Animation Pack
    public void LoadAnimationPack()
    {
        // If in UnityEditor, load the asset file from a path. Use relative path to project folder.
        if (Application.isEditor)
        {
            string path = UnityEditor.EditorUtility.OpenFilePanel("Load Animation Pack", Application.dataPath, "asset");
            if (path.Length != 0)
            {
                path = path.Replace(Application.dataPath, "Assets");
                loadedSparrowAnimationPack = UnityEditor.AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(path);
                editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);
            }
            UpdateAnimationList();
        }
    }

    // Save Animation Pack
    public void SaveAnimationPack()
    {
        // If in UnityEditor, override the original asset file. Save with original path.

        if (Application.isEditor)
        {
            Debug.Log("Saving Animation Pack");
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(loadedSparrowAnimationPack);
            if (assetPath != string.Empty)
            {
                UnityEditor.AssetDatabase.CreateAsset(editingSparrowsAnimationPack, assetPath);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

                Debug.Log("Saved Animation Pack");
            }
            else
            {
                Debug.Log("File does not exist. Saving as new file.");
                SaveAnimationPackAs();
            }
        }
    }

    // Save Animation Pack As
    public void SaveAnimationPackAs()
    {
        // If in UnityEditor, save the asset file to a new path.
        if (Application.isEditor)
        {
            string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Animation Pack", "New Animation Pack", "asset", "Please enter a file name to save the animation pack to");
            Debug.Log("Save Animation Pack As: " + path);
            if (path.Length != 0)
            {
                UnityEditor.AssetDatabase.CreateAsset(editingSparrowsAnimationPack, path);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

                loadedSparrowAnimationPack = UnityEditor.AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(path);
                editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);

                Debug.Log("Saved Animation Pack As");
            }
        }
    }
    #endregion

    public void toggleGhostSparrow(bool toggle)
    {
        ghostSparrowRenderer.gameObject.SetActive(toggle);
    }

    #region Animation Management
    public void CreateAnimation()
    {
        Debug.Log("Create Animation");
        UpdateAnimationList();
    }

    public void LoadAnimation()
    {
        Debug.Log("Load Animation");
    }

    public void SaveAnimation()
    {
        Debug.Log("Save Animation");
        UpdateAnimationList();
    }

    public void DeleteAnimation()
    {
        Debug.Log("Delete Animation");
        UpdateAnimationList();
    }
    #endregion

    public void ChangedSelectedAnimation(int index)
    {
        Extrite.Animation selectedAnimation = editingSparrowsAnimationPack.animations[index];
        Debug.Log("Selected Animation: " + selectedAnimation.name);

        mainSparrowRenderer.sparrowAnimationPack = editingSparrowsAnimationPack;
        mainSparrowRenderer.Play(selectedAnimation.name);

        animationNameInputField.text = selectedAnimation.name;
        prefixInputField.text = selectedAnimation.prefix;
        fpsInputField.text = selectedAnimation.fps.ToString();
        loopToggle.isOn = selectedAnimation.loop;
    }

    public void SaveCurrentAnimation()
    {
        Extrite.Animation newAnimation = new Extrite.Animation();
        newAnimation.name = animationNameInputField.text;
        newAnimation.prefix = prefixInputField.text;
        newAnimation.fps = Convert.ToInt32(fpsInputField.text);
        newAnimation.loop = loopToggle.isOn;

        // Check if theirs an animation with the same name, if so, replace it. If not, add it.
        bool found = false;
        for (int i = 0; i < editingSparrowsAnimationPack.animations.Length; i++)
        {
            if (editingSparrowsAnimationPack.animations[i].name == newAnimation.name)
            {
                editingSparrowsAnimationPack.animations[i] = newAnimation;
                found = true;
                break;
            }
        }

        if (!found)
        {
            Extrite.Animation[] newAnimations = new Extrite.Animation[editingSparrowsAnimationPack.animations.Length + 1];
            for (int i = 0; i < editingSparrowsAnimationPack.animations.Length; i++)
            {
                newAnimations[i] = editingSparrowsAnimationPack.animations[i];
            }
            newAnimations[newAnimations.Length - 1] = newAnimation;
            editingSparrowsAnimationPack.animations = newAnimations;
        }

        UpdateAnimationList();
    }

    void UpdateAnimationList()
    {
        animationDropdown.ClearOptions();

        if (editingSparrowsAnimationPack.animations == null)
        {
            return;
        }

        foreach (Extrite.Animation animation in editingSparrowsAnimationPack.animations)
        {
            animationDropdown.options.Add(new TMP_Dropdown.OptionData(animation.name));
        }

        animationDropdown.value = -1;
    }
    
}
