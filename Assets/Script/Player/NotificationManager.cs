using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    private float startPosX;
    private float targetPosX = 0f;
    private TextMeshProUGUI notificationText;

    void Start()
    {
        startPosX = transform.position.x;
        notificationText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Notification(string text, float seconds = 5f)
    {
        Timing.RunCoroutine(NotificationCoroutine(text, seconds));
    }

    private IEnumerator<float> NotificationCoroutine(string text, float seconds)
    {
        float wait = Timing.WaitForSeconds(seconds);
        SetNotificationText(text);
        ShowNotification();
        yield return wait;
        HideNotification();
    }

    private void SetNotificationText(string text) => notificationText.text = text;

    private void ShowNotification()
    {
        transform.DOMoveX(targetPosX, .2f);
    }

    private void HideNotification()
    {
        transform.DOMoveX(startPosX, .2f);
    }
}
