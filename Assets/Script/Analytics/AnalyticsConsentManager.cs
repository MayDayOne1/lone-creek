using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsConsentManager : MonoBehaviour
{
    public GameObject consentWindow;

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
        int consentInt = PlayerPrefs.GetInt("isConsentGiven");
        // Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));

        if(LoadFirstTimeOnThisMachine() == 0)
        {
            ShowConsentWindow(true);
            SetFirstTimeOnThisMachine();
        }
        if (consentInt == 1)
        {
            AnalyticsService.Instance.StartDataCollection();
        }
    }

    public void OptIn()
    {
        AnalyticsService.Instance.StartDataCollection();
        PlayerPrefs.SetInt("isConsentGiven", 1);
        PlayerPrefs.Save();
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));
        ShowConsentWindow(false);
    }

    public void OptOut()
    {
        AnalyticsService.Instance.StopDataCollection();
        PlayerPrefs.SetInt("isConsentGiven", 0);
        PlayerPrefs.Save();
        
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt("isConsentGiven"));
        ShowConsentWindow(false);
    }

    public void ShowConsentWindow(bool show)
    {
        consentWindow.SetActive(show);
    }

    private void SetFirstTimeOnThisMachine()
    {
        PlayerPrefs.SetInt("firstTime", 1);
    }

    private int LoadFirstTimeOnThisMachine()
    {
        return PlayerPrefs.GetInt("firstTime");
    }
}
