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
            associatedApp.Open();
        }
    }

    public void Setup(NotificationCenter.Notification notification)
    {
        associatedApp = notification.associatedApp;
        //ERROR AL CARGAR UNA PARTIDA Y QUE LLEGUE UNA NOTIF
        image.sprite = notification.sprite;
        title.text = notification.title;
        content.text = notification.content;
    }
}
