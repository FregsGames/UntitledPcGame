using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NotificationCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI content;
    [SerializeField]
    private Image image;
    [SerializeField]
    private RectTransform rect;

    private App associatedApp;

    public RectTransform Rect { get => rect; set => rect = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(associatedApp != null)
        {
            // open app
        }
    }

    public void Setup(NotificationCenter.Notification notification)
    {
        associatedApp = notification.associatedApp;
        image.sprite = notification.sprite;
        title.text = notification.title;
        content.text = notification.content;
    }
}
