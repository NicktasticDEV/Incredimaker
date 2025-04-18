using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incredimaker
{
    public class ModListFetchTest : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("Fetching mods...");
            List<ModData> mods = ModDataLoader.FetchMods();
            Debug.Log("Fetched " + mods.Count + " mods.");
            foreach (ModData mod in mods)
            {
                Debug.Log("===== Mod =====");
                Debug.Log("Mod name: " + mod.name);
                Debug.Log("Mod description: " + mod.description);
                Debug.Log("Mod version: " + mod.version);
            }
        }
    }
}