using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGridUI : MonoBehaviour
{
    [SerializeField]
    private GameObject CharacterIconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        // For each character in the mod manager, create a new character icon
        foreach (Mod mod in ModManager.Instance.mods)
        {
            foreach (Character character in mod.characters)
            {
                if (character.name == "Default") { continue; }
                GameObject characterIcon = Instantiate(CharacterIconPrefab, transform);
                characterIcon.GetComponent<CharacterIcon>().regularIcon = character.thumbnail;
                characterIcon.GetComponent<CharacterIcon>().characterName = character.name;
            }
        }
        
    }
}
