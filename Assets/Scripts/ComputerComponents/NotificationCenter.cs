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
    [SerializeField]
    private SpritesDB spriteDB;

    private NotificationCard notificationCard;
    private Queue<Notification> notificationQueue = new Queue<Notification>();

    private bool showingNotification;

    public void Initialize()
    {
        notificationCard = Instantiate(notificationCardPrefab, Computer.Instance.Desktop.transform).GetComponent<NotificationCard>();
    }

    public void RequestNotification(string sprite, string title, string content, App associatedApp)
    {
        if(notificationCard == null)
        {
            notificationCard = Instantiate(notificationCardPrefab, Computer.Instance.Desktop.transform).GetComponent<NotificationCard>();
        }

        Notification notification = new Notification()
        {
            content = content,
            title = title,
            sprite = spriteDB.GetSprite(sprite),
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
        SoundManager.Instance.PlaySound(SoundManager.Sound.Notification);

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

