using Sirenix.OdinInspector;
using UnityEngine;

public class App : UniqueID, ISaveableState
{
    public enum AppType
    {
        Folder = 1,
        TextFile = 2,
        Desktop = 3,
        LockedFolder = 4,
        LockedTextFile = 5,
        ConfirmationPopup = 6,
        StringPopup = 7,
        NumericPopup = 8,
        Settings = 9,
        Alarms = 10,
        SecurityCameras = 11,
        OkPopup = 12,
        CameraController = 13
    }
    [SerializeField]
    protected AppType type;
    [SerializeField]
    protected SystemEventManager systemEventManager;
    [SerializeField]
    protected UIEventManager uiEventManager;
    [SerializeField]
    protected Transform root;

    public AppType Type { get => type; set => type = value; }

    public Transform Root { get => root; }
    public virtual void Open()
    {
        systemEventManager.OnAppOpen?.Invoke(this);
        Serialize();
    }

    public virtual void Close()
    {
        Serialize();
        systemEventManager.OnAppClosed?.Invoke(ID);


        Destroy(gameObject);
    }

    public virtual void RecenterOnUI()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(Screen.width - rectTransform.rect.width, Screen.height - rectTransform.rect.height) / 2;
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            Open();
        }
    }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }

    public virtual void Serialize()
    {
    }

    public virtual void Deserialize()
    {

    }
}
