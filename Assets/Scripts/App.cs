using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class App : UniqueID, ISaveableState
{
    public enum AppType
    {
        Folder = 1,
        TextFile = 2,
        Desktop = 3,
        LockedFolder = 4
    }
    [SerializeField]
    protected AppType type;
    [SerializeField]
    protected SystemEventManager systemEventManager;
    
    public AppType Type { get => type; set => type = value; }


    public virtual void Open()
    {
        systemEventManager.OnAppOpen?.Invoke(this);
    }

    public virtual void Close()
    {
        systemEventManager.OnAppClosed?.Invoke(ID);
    }

    public virtual void RecenterOnUI()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(Screen.width - rectTransform.rect.width, Screen.height - rectTransform.rect.height)/2;
    }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }

    public virtual Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>();
    }

    public virtual void Deserialize()
    {

    }
}
