using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Icon : UniqueID, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, ICustomSerializable
{
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private Image image;
    [SerializeField]
    private EventManager eventManager;
    [SerializeField]
    private TextMeshProUGUI iconName;

    // Properties
    public IconsContainer Container { get; set; }
    public Vector3 Position { get => rect.position; }

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

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (Input.mousePosition != originalPos && Container != null)
        {
            IconsContainer newContainer = GetContainerUnderMouse();
            {
                if (newContainer != null)
                {
                    if (newContainer.MoveIconTo(this, Input.mousePosition) && Container != newContainer)
                    {
                        Container.RemoveIconIfAlreadyExists(this);
                        Container = newContainer;
                    }
                    return;
                }

                Container.MoveIconTo(this, Input.mousePosition);
            }
        }
    }

    protected IconsContainer GetContainerUnderMouse()
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

    public Dictionary<string, string> Serialize()
    {
        Dictionary<string, string> serialized = new Dictionary<string, string>();

        serialized.Add($"{ID}_sprite", SpriteManager.Instance.SpritesDB.GetID(Sprite));
        serialized.Add($"{ID}_text", Text);

        return serialized;
    }

    public void Deserialize()
    {

        string spriteKey = SaveManager.instance.RetrieveString($"{ID}_sprite");
        string textValue = SaveManager.instance.RetrieveString($"{ID}_text");

        image.sprite = SpriteManager.Instance.SpritesDB.GetSprite(spriteKey);
        iconName.text = textValue;
    }

    // Odin Stuff
    [PropertySpace(10, 0)]
    [Button("New ID", ButtonSizes.Medium)]
    protected void RegenerateID()
    {
        RegenerateGUID();
    }
}
