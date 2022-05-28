using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static App;

public class Icon : UniqueID, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, ISaveableState
{
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private Image image;
    [SerializeField]
    private UIEventManager eventManager;
    [SerializeField]
    private TextMeshProUGUI iconName;
    [SerializeField]
    private bool immovable;
    [SerializeField]
    private GameObject backgroundHover;

    [Header("Associated app")]
    [SerializeField]
    private App associatedApp;
    [SerializeField]
    private AppType associatedAppType;
    [SerializeField]
    private string associatedAppID;

    // Properties
    public IconsContainer Container { get; set; }
    public Vector3 Position { get => rect.position; }
    public App AssociatedApp { get => associatedApp; }
    public AppType AssociatedAppType { get => associatedAppType; }
    public GameObject BackgroundHover { get => backgroundHover; }
    private bool HasMoved { get => Input.mousePosition != originalPos && Container != null && dragging; }

    // Persistence
    public Sprite Sprite { get => image.sprite; }
    public string Text { get => iconName.text; }
    public Color SpriteColor { get => image.color; }

    // Drag
    private bool dragging = false;
    protected Vector3 originalPos = Vector3.zero;

    private void OnEnable()
    {
        eventManager.OnUIScaleChanged += UpdateSize;
    }

    private void UpdateSize(float uiScale)
    {
        rect.sizeDelta = uiScale * ComputerScreen.Instance.IconsBaseSize;
        iconName.fontSize = uiScale * ComputerScreen.Instance.DefaultIconTextSize;
    }

    public void SetPos(Vector3 pos)
    {
        rect.position = pos;
        originalPos = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && !dragging)
        {
            if (associatedApp != null)
            {
                associatedApp.RecenterOnUI();
            }
            else
            {
                associatedApp = InstantiatorManager.Instance.Instantiate(associatedAppType, associatedAppID).GetComponentInChildren<App>();
                associatedApp.Open();

                if (associatedAppID == string.Empty)
                {
                    associatedAppID = associatedApp.ID;
                    Serialize();
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (HasMoved)
        {
            IconsContainer newContainer = ContainerUnderMouse();
            if (newContainer != null)
            {
                if ((immovable && newContainer != Container) || (newContainer == associatedApp))
                    return;

                if (newContainer.MoveIconTo(this, Input.mousePosition) && Container != newContainer)
                {
                    Container.RemoveIconIfAlreadyExists(this);
                    Container = newContainer;
                }
            }
            else
            {
                Container.MoveIconTo(this, Input.mousePosition);
            }
        }
    }

    protected IconsContainer ContainerUnderMouse()
    {
        IconsContainer container = null;
        var m_PointerEventData = new PointerEventData(EventSystem.current);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = ComputerScreen.Instance.GraphicRaycaster;
        raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            container = results.FirstOrDefault(r => r.gameObject.GetComponent<IconsContainer>() != null).gameObject.GetComponent<IconsContainer>();
        }

        return container;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPos = Position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
        {
            dragging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        eventManager.OnEndStartDrag?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventManager.OnIconStartDrag?.Invoke(this);
    }

    public virtual void Serialize()
    {
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}_sprite", SpriteManager.Instance.SpritesDB.GetID(Sprite));
        serialized.Add($"{ID}_text", Text);
        serialized.Add($"{ID}_immovable", immovable ? "true" : "false");
        if (associatedAppID != string.Empty)
        {
            serialized.Add($"{ID}_associatedId", associatedAppID);
        }

        serialized.Add($"{ID}_associatedTypeOf", associatedAppType.ToString());

        SaveManager.Instance.Save(serialized, ID);
    }

    public virtual void Deserialize()
    {
        string spriteKey = SaveManager.Instance.RetrieveString($"{ID}_sprite");
        string textValue = SaveManager.Instance.RetrieveString($"{ID}_text");
        string typeOfAssociated = SaveManager.Instance.RetrieveString($"{ID}_associatedTypeOf");

        if (typeOfAssociated != string.Empty)
        {
            associatedAppType = (AppType)Enum.Parse(typeof(AppType), SaveManager.Instance.RetrieveString($"{ID}_associatedTypeOf"));
        }

        associatedAppID = SaveManager.Instance.RetrieveString($"{ID}_associatedId");
        immovable = SaveManager.Instance.RetrieveString($"{ID}_immovable").Equals("true");
        image.sprite = SpriteManager.Instance.SpritesDB.GetSprite(spriteKey);
        iconName.text = textValue;

#if UNITY_EDITOR
        gameObject.name = textValue;
#endif
    }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }
}