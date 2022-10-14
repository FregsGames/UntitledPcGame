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
        CameraController = 13,
        Mail = 14,
        FirstVirus = 15,
        Antivirus = 16
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
        RectTransform rectTransform = root.GetComponent<RectTransform>();

        rectTransform.position = new Vector3((ComputerScreen.Instance.BackgroundSize.x - rectTransform.sizeDelta.x) * ComputerScreen.Instance.ScreenRelation.x / 2,
            (ComputerScreen.Instance.BackgroundSize.y + rectTransform.sizeDelta.y) * ComputerScreen.Instance.ScreenRelation.y / 2, 0);

        if (!gameObject.activeInHierarchy)
        {
            Deserialize();
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
        SaveManager.Instance.Save($"{ID}_size_x", root.GetComponent<RectTransform>().sizeDelta.x);
        SaveManager.Instance.Save($"{ID}_size_y", root.GetComponent<RectTransform>().sizeDelta.y);
    }

    public virtual void Deserialize()
    {
        var savedXSize = SaveManager.Instance.RetrieveFloat($"{ID}_size_x", -1);
        var savedYSize = SaveManager.Instance.RetrieveFloat($"{ID}_size_y", -1);

        if(savedXSize > 0 && savedYSize > 0)
        {
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(savedXSize, savedYSize);
        }
    }
}
