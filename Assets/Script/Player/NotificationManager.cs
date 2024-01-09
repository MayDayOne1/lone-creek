using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine.SceneManagement;

public class NotificationManager : MonoBehaviour
{
    private bool isNotifying = false;
    private float startPosX;
    private float targetPosX = 0f;
    private TextMeshProUGUI notificationText;
    private Queue<string> notificationQueue = new();

    private static bool firstWeaponPickup = true;
    private static bool secondWeaponPickup = true;

    void Start()
    {
        startPosX = transform.position.x;
        notificationText = GetComponentInChildren<TextMeshProUGUI>();

        LevelStartMessage();
    }

    public void WeaponNotification()
    {
        if (firstWeaponPickup)
        {
            AddToQueue("Press RMB to aim. Press LMB to fire/throw.");
            firstWeaponPickup = false;
        }
        else if(secondWeaponPickup)
        {
            AddToQueue("Press 1 or 2 to switch weapons.");
            secondWeaponPickup = false;
        }
        DebugQueue();
        Notification();
    }

    private void AddToQueue(string text)
    {
        foreach(string item in notificationQueue)
        {
            if (item == text)
            {
                return;
            }
        }
        notificationQueue.Enqueue(text);
    }

    private void DebugQueue()
    {
        Debug.Log(notificationQueue.Count);
        foreach (string text in  notificationQueue)
        {
            Debug.Log(text);
        }
    }

    private void Notification(float seconds = 5f)
    {
        if(!isNotifying)
        {
            Timing.RunCoroutine(NotificationCoroutine(seconds));
        }
    }

    private void LevelStartMessage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            AddToQueue("Get to the exit!");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            AddToQueue("Get to the blue car!");
        }
        Notification();
    }

    private IEnumerator<float> NotificationCoroutine(float seconds)
    {
        isNotifying = true;
        float wait = Timing.WaitForSeconds(seconds);

        if(notificationQueue.Count > 0)
        {
            string item = notificationQueue.Dequeue();
            SetNotificationText(item);
            ShowNotification();
            yield return wait;
            HideNotification();
            if(notificationQueue.Count > 0)
            {
                Timing.RunCoroutine(NotificationCoroutine(seconds));
            }
        }
        isNotifying = false;
    }

    private void SetNotificationText(string text) => notificationText.text = text;

    private void ShowNotification(float time = .2f)
    {
        transform.DOMoveX(targetPosX, time);
    }

    private void HideNotification(float time = .2f)
    {
        transform.DOMoveX(startPosX, time);
    }
}
