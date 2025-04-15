using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModManager : MonoBehaviour
{
    // Mod manager will look into mod folder and turn all mod stuff into scriptable objects so that their are accessable anytime
    // Mod manager should also be able to unload mods by deleting all scriptable objects assosiated with the mod thats being unloaded

    // To turn stuff from mods folder into scriptable objects, we'll probably have to write some custom importer, so for example,
    // getting the path of the audio file from the json and loading it as an audioclip to use in unity.
}
