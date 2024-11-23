using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpriteTestUI : MonoBehaviour
{
    public TMP_Dropdown characterDropdown;


    void Awake()
    {
        characterDropdown.onValueChanged.AddListener(characterDropdownValueChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Populate dropdown with character names including a none option at beginning
        List<string> characterNames = new List<string>();
        characterNames.Add("None");

        foreach (Character character in ModManager.Instance.mods[0].characters)
        {
            characterNames.Add(character.name);
        }

        characterDropdown.AddOptions(characterNames);
    }

    private void characterDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            // Reset character selection
            return;
        }

        // Set selected character
        Character selectedCharacter = ModManager.Instance.mods[0].characters[index - 1];
        PlayManager.instance.characters[0].selectedCharacter = selectedCharacter;
    }
}
