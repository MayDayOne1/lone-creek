using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class InitateAnalytics : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (ConsentCheckException e)
        {
            Debug.Log(e.ToString());
        }
        AnalyticsService.Instance.StartDataCollection();
    }
}
