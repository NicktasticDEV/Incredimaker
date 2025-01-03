using System;
using Extrite;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
    public TMP_Dropdown animationDropdown;
    [Header("Animation Management UI")]
    public TMP_Dropdown ghostAnimationDropdown;
    // Input Fields
    public TMP_InputField animationNameInputField;
    public TMP_InputField prefixInputField;
    public TMP_InputField fpsInputField;
    public Toggle loopToggle;
    public Toggle globalOffsetToggle;
    public Toggle animOffsetToggle;


    [Header("Misc")]
    public bool currentlyWritingFile = false;
    private string externalWorkPath = "";
    public int offsetIncrement = 1;

    void Awake()
    {
        editingSparrowsAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();

        // Events
        ghostSparrowToggle.onValueChanged.AddListener(toggleGhostSparrow);

        animationDropdown.onValueChanged.AddListener(ChangedSelectedAnimation);
        ghostAnimationDropdown.onValueChanged.AddListener(ChangedSelectedGhostAnimation);
    }

    void Start()
    {

    }

    void Update()
    {
        // If any of the arrow keys are pressed, call the KeyboardPress function
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            KeyboardPress();
        }

        // If shift is pressed, make the offset increment 5
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            offsetIncrement = 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            offsetIncrement = 1;
        }
    }

    #region Animation Pack File Management
    // New Animation Pack
    public void NewAnimationPack()
    {
        editingSparrowsAnimationPack = ScriptableObject.CreateInstance<SO_SparrowAnimationPack>();
        UpdateAnimationList();
    }

    // Load Animation Pack
    public void LoadAnimationPackInternal()
    {
        #if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Load Animation Pack", Application.dataPath, "asset");
        if (path.Length != 0)
        {
            path = path.Replace(Application.dataPath, "Assets");
            loadedSparrowAnimationPack = UnityEditor.AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(path);
            editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);
        }
        UpdateAnimationList();
        #endif
    }

    public void LoadAnimationPackExternal()
    {
        string path = Extrite.Utilities.GetPathFromDialogue("Load Animation Pack", "esap", false);

        loadedSparrowAnimationPack = Extrite.Utilities.ImportSparrowAnimationPack(path);
        editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);
        externalWorkPath = path;
        UpdateAnimationList();
    }

    // Save Animation Pack Externally
    public void SaveAnimationPackExternal(bool saveAs)
    {
        if (externalWorkPath.Length == 0 || saveAs)
        {
            string path = Extrite.Utilities.GetPathFromDialogue("Save Animation Pack", "esap", true);
            if (path.Length != 0)
            {
                Extrite.Utilities.ExportSparrowAnimationPack(editingSparrowsAnimationPack, path);
                externalWorkPath = path;
            }
        }
        else
        {
            Extrite.Utilities.ExportSparrowAnimationPack(editingSparrowsAnimationPack, externalWorkPath);
        }
    }

    // Save Animation Pack Internally in the projects assets
    public void SaveAnimationPackInternal(bool saveAs)
    {
        #if UNITY_EDITOR
        // Check if the loaded animation pack we're dealing with is actually a file in the project, cause it may have been loaded externally
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(loadedSparrowAnimationPack);

        if (assetPath.Length == 0 || saveAs)
        {
            string path = UnityEditor.EditorUtility.SaveFilePanel("Save Animation Pack", Application.dataPath, "New Animation Pack", "asset");
            if (path.Length != 0)
            {
                path = path.Replace(Application.dataPath, "Assets");
                UnityEditor.AssetDatabase.CreateAsset(editingSparrowsAnimationPack, path);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

                // Load the saved file
                loadedSparrowAnimationPack = UnityEditor.AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(path);
                editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);

                UpdateAnimationList();
            }
        }
        else
        {
            // Transfer the data from the editing animation pack to the loaded animation pack and save it
            loadedSparrowAnimationPack.animations = editingSparrowsAnimationPack.animations;
            loadedSparrowAnimationPack.globalOffset = editingSparrowsAnimationPack.globalOffset;
            UnityEditor.EditorUtility.SetDirty(loadedSparrowAnimationPack);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            loadedSparrowAnimationPack = UnityEditor.AssetDatabase.LoadAssetAtPath<SO_SparrowAnimationPack>(assetPath);
            editingSparrowsAnimationPack = Instantiate(loadedSparrowAnimationPack);

            UpdateAnimationList();
        }

        #endif

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
        // Clear all the input fields from Animation Management UI
        animationNameInputField.text = "";
        prefixInputField.text = "";
        fpsInputField.text = "";
        loopToggle.isOn = false;
    }

    public void SaveAnimation()
    {
        Debug.Log("Save Animation");
        Extrite.Animation newAnimation = new Extrite.Animation();
        newAnimation.name = animationNameInputField.text;
        newAnimation.prefix = prefixInputField.text;
        newAnimation.fps = Convert.ToInt32(fpsInputField.text);
        newAnimation.loop = loopToggle.isOn;

        // If animation with the same name already exists, update it, otherwise, create a new array with the already existing animations and add the new one and assign the new array to the animations array
        if (editingSparrowsAnimationPack.animations == null)
        {
            editingSparrowsAnimationPack.animations = new Extrite.Animation[1];
            editingSparrowsAnimationPack.animations[0] = newAnimation;
        }
        else
        {
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
                newAnimations[editingSparrowsAnimationPack.animations.Length] = newAnimation;
                editingSparrowsAnimationPack.animations = newAnimations;
            }
        }


        UpdateAnimationList();
    }

    public void DeleteAnimation()
    {
        Debug.Log("Delete Animation");
        if (editingSparrowsAnimationPack.animations == null)
        {
            return;
        }

        for (int i = 0; i < editingSparrowsAnimationPack.animations.Length; i++)
        {
            if (editingSparrowsAnimationPack.animations[i].name == animationNameInputField.text)
            {
                Extrite.Animation[] newAnimations = new Extrite.Animation[editingSparrowsAnimationPack.animations.Length - 1];
                for (int j = 0; j < i; j++)
                {
                    newAnimations[j] = editingSparrowsAnimationPack.animations[j];
                }
                for (int j = i + 1; j < editingSparrowsAnimationPack.animations.Length; j++)
                {
                    newAnimations[j - 1] = editingSparrowsAnimationPack.animations[j];
                }
                editingSparrowsAnimationPack.animations = newAnimations;
                break;
            }
        }

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

    public void ChangedSelectedGhostAnimation(int index)
    {
        Extrite.Animation selectedAnimation = editingSparrowsAnimationPack.animations[index];
        Debug.Log("Selected Ghost Animation: " + selectedAnimation.name);

        ghostSparrowRenderer.sparrowAnimationPack = editingSparrowsAnimationPack;
        ghostSparrowRenderer.Play(selectedAnimation.name);
    }

    void UpdateAnimationList()
    {
        animationDropdown.ClearOptions();
        ghostAnimationDropdown.ClearOptions();

        if (editingSparrowsAnimationPack.animations == null)
        {
            return;
        }

        foreach (Extrite.Animation animation in editingSparrowsAnimationPack.animations)
        {
            animationDropdown.options.Add(new TMP_Dropdown.OptionData(animation.name));
            ghostAnimationDropdown.options.Add(new TMP_Dropdown.OptionData(animation.name));
        }

        animationDropdown.value = -1;
        ghostAnimationDropdown.value = -1;
    }
    
    void KeyboardPress()
    {
        if (globalOffsetToggle.isOn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.globalOffset.y += offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.globalOffset.y -= offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.globalOffset.x -= offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.globalOffset.x += offsetIncrement;
            }
            // Play the animation again to update the position with the current selected animation in the UI
            mainSparrowRenderer.Play(animationDropdown.options[animationDropdown.value].text);
        }
        else if (animOffsetToggle.isOn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.animations[animationDropdown.value].offset.y += offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.animations[animationDropdown.value].offset.y -= offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.animations[animationDropdown.value].offset.x -= offsetIncrement;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                mainSparrowRenderer.sparrowAnimationPack.animations[animationDropdown.value].offset.x += offsetIncrement;
            }
            // Play the animation again to update the position with the current selected animation in the UI
            mainSparrowRenderer.Play(animationDropdown.options[animationDropdown.value].text);
        }


    }

}
