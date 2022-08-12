using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AppResizer : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private Vector2 direction;

    [SerializeField]
    private Vector2Int minSize = new Vector2Int(800,700);
    [SerializeField]
    private Vector2Int maxSize = new Vector2Int(1200, 1000);

    private RectTransform rect;

    private void Start()
    {
        rect = transform.parent.GetComponent<RectTransform>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        rect.sizeDelta += new Vector2(direction.x * eventData.delta.x / ComputerScreen.Instance.ScreenRelation.x, direction.y * eventData.delta.y / ComputerScreen.Instance.ScreenRelation.y);

        rect.sizeDelta = new Vector2(Mathf.Clamp(rect.sizeDelta.x, minSize.x, maxSize.x), Mathf.Clamp(rect.sizeDelta.y, minSize.y, maxSize.y));
    }
}
