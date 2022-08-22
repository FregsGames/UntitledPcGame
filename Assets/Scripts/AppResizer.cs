using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AppResizer : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private Vector2 direction;

    [SerializeField]
    private Vector2Int minSize = new Vector2Int(800, 700);
    [SerializeField]
    private Vector2Int maxSize = new Vector2Int(1200, 1000);

    public UnityEvent onResize;

    private RectTransform rect;

    [SerializeField]
    private string cursorId;

    [SerializeField]
    private SpritesDB sprites;

    private static bool dragging;
    private static bool hovering;


    private void Start()
    {
        rect = transform.parent.GetComponent<RectTransform>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        rect.sizeDelta += new Vector2(direction.x * eventData.delta.x / ComputerScreen.Instance.ScreenRelation.x, direction.y * eventData.delta.y / ComputerScreen.Instance.ScreenRelation.y);

        rect.sizeDelta = new Vector2(Mathf.Clamp(rect.sizeDelta.x, minSize.x, maxSize.x), Mathf.Clamp(rect.sizeDelta.y, minSize.y, maxSize.y));
        onResize?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        if (dragging) return;

        Cursor.SetCursor(sprites.GetTexture(cursorId), Vector2.one * 32, CursorMode.ForceSoftware);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        if (!dragging)
        {
            Cursor.SetCursor(sprites.GetTexture("defaultCursor"), Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        Cursor.SetCursor(sprites.GetTexture(cursorId), Vector2.one * 32, CursorMode.ForceSoftware);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;

        if (!hovering)
        {
            Cursor.SetCursor(sprites.GetTexture("defaultCursor"), Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
