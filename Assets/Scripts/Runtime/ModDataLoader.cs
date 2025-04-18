using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incredimaker
{
    public class ModDataLoader
    {
        public static List<ModData> FetchMods()
        {
            // Mods are located in streamingassets/mods
            // in that folder are folders for each mod
            // in each mod folder is a mod.json file

            // return a list of mods that are in the folder
            string[] modFolders = System.IO.Directory.GetDirectories(Application.streamingAssetsPath + "/mods");
            List<ModData> mods = new List<ModData>();
            foreach (string modFolder in modFolders)
            {
                string modJsonPath = System.IO.Path.Combine(modFolder, "mod.json");
                if (System.IO.File.Exists(modJsonPath))
                {
                    string json = System.IO.File.ReadAllText(modJsonPath);
                    ModData modData = JsonUtility.FromJson<ModData>(json);
                    mods.Add(modData);
                }
            }
            return mods;
        }
    }
}
