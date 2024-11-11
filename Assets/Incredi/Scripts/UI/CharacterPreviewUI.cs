using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterPreviewUI : MonoBehaviour
{

    // Lists
    [SerializeField] private TMP_Dropdown modSelector;
    [SerializeField] private TMP_Dropdown characterSelector;

    // Buttons
    [SerializeField] private Button modSelectButton;
    [SerializeField] private Button characterSelectButton;

    private Mod selectedMod;

    void Awake()
    {
        modSelectButton.onClick.AddListener(() =>
        {
            modSelected(modSelector.value);
        });

        characterSelectButton.onClick.AddListener(() =>
        {
            characterSelected(characterSelector.value);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        modSelector.ClearOptions();

        List<string> modNames = new List<string>();
        foreach (Mod mod in ModManager.Instance.mods)
        {
            modNames.Add(mod.metadata.modName);
        }

        modSelector.AddOptions(modNames);
    }

    private void modSelected(int index)
    {
        selectedMod = ModManager.Instance.mods[index];
        Debug.Log("Selected mod: " + selectedMod.metadata.modName);
    }

    private void characterSelected(int index)
    {
        Debug.Log("Selected character: " + index);
    }

}
