using Sirenix.OdinInspector;
using UnityEngine;

public class App : UniqueID
{
    public enum AppType
    {
        Folder,
        TextFile,
        Desktop
    }

    [SerializeField]
    protected AppType type;

    public AppType Type { get => type; }


    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }

}
