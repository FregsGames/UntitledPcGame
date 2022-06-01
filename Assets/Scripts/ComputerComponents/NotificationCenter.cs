using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class NotificationCenter : MonoBehaviour
{
    [SerializeField]
    private GameObject notificationCardPrefab;

    private NotificationCard notificationCard;
    private Queue<Notification> notificationQueue = new Queue<Notification>();

    private bool showingNotification;

    private async void Start()
    {
        await Task.Delay(1000);
        notificationCard = Instantiate(notificationCardPrefab, Computer.Instance.Desktop.transform).GetComponent<NotificationCard>();
    }

    public void RequestNotification(Sprite sprite, string title, string content, App associatedApp)
    {
        Notification notification = new Notification()
        {
            content = content,
            title = title,
            sprite = sprite,
            associatedApp = associatedApp
        };

        notificationQueue.Enqueue(notification);

        if (!showingNotification)
        {
            ShowNotification();
        }
    }

    private async void ShowNotification()
    {
        showingNotification = true;
        Notification notification = notificationQueue.Dequeue();
        notificationCard.Setup(notification);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(notificationCard.Rect.DOAnchorPosY(-200, 0.8f));
        sequence.AppendInterval(2);
        sequence.Append(notificationCard.Rect.DOAnchorPosY(0, 1));

        await sequence.AsyncWaitForCompletion();

        showingNotification = false;
        if(notificationQueue.Count > 0)
        {
            ShowNotification();
        }
    }

    public struct Notification
    {
        public string title;
        public string content;
        public Sprite sprite;
        public App associatedApp;
    }
}

