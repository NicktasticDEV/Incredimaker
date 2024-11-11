// Mod metadata format
[System.Serializable]
public class ModMetadata
{
    public string version;
    public string modName;
    public string modDescription;
}

[System.Serializable]
public class Mod
{
    public ModMetadata metadata;
    public string path;

    public Mod(ModMetadata metadata)
    {
        this.metadata = metadata;
    }
}
