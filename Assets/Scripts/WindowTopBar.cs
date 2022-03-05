using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowTopBar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private RectTransform window;

    // Drag
    private Vector3 dragOffset;
    public Action OnWindowEndDrag;

    public App App { get; set; }

    private void Start()
    {
        App = window.GetComponentInChildren<App>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.mousePosition.x < Screen.width &&
            Input.mousePosition.y < Screen.height &&
            Input.mousePosition.y > ComputerScreen.Instance.BottomBarHeight &&
            Input.mousePosition.x > 0)
        {
            window.position = Input.mousePosition - dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnWindowEndDrag?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragOffset = Input.mousePosition - window.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
