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
            Container.MoveIconTo(this, Input.mousePosition);
        }
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
