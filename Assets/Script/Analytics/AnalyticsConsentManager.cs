using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsConsentManager : MonoBehaviour
{
    public GameObject consentWindow;
    public static bool? isConsentGiven = null;

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
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));
        int consentInt = PlayerPrefs.GetInt("isConsentGiven");
        if (consentInt == 0) isConsentGiven = false;
        else if (consentInt == 1)
        {
            isConsentGiven = true;
            AnalyticsService.Instance.StartDataCollection();
        }
        else isConsentGiven = null;

        if (isConsentGiven == null)
        {
            ShowConsentWindow(true);
        } else
        {
            ShowConsentWindow(false);
        }
    }

    public void OptIn()
    {
        AnalyticsService.Instance.StartDataCollection();
        isConsentGiven = true;
        PlayerPrefs.SetInt("isConsentGiven", 1);
        PlayerPrefs.Save();
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));
        ShowConsentWindow(false);
    }

    public void OptOut()
    {
        if(isConsentGiven == true)
        {
            AnalyticsService.Instance.StopDataCollection();
            isConsentGiven = false;
            PlayerPrefs.SetInt("isConsentGiven", 0);
            PlayerPrefs.Save();
        }
        
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));
        ShowConsentWindow(false);
    }

    public void ShowConsentWindow(bool show)
    {
        consentWindow.SetActive(show);
    }
}
