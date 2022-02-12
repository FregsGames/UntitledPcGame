using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private Image image;
    [SerializeField]
    private EventManager eventManager;

    // Properties
    public IconsContainer Container { get; set; }
    public Vector3 Position { get => rect.position; }
    public Sprite Sprite { get => image.sprite; }
    public Color SpriteColor { get => image.color; }

    // Drag
    private bool dragging = false;
    private Vector3 originalPos = Vector3.zero;

    private void OnEnable()
    {

    }

    public void SetPos(Vector3 pos)
    {
        rect.position = pos;
        originalPos = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.mousePosition != originalPos && Container != null)
        {
            IconsContainer newContainer = GetContainerUnderMouse();
            {
                if (newContainer != null)
                {
                    if (newContainer.MoveIconTo(this, Input.mousePosition) && Container != newContainer)
                    {
                        Container.RemoveIcon(this);
                        Container = newContainer;
                    }
                    return;
                }

                Container.MoveIconTo(this, Input.mousePosition);
            }
        }
    }

    private IconsContainer GetContainerUnderMouse()
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
}
