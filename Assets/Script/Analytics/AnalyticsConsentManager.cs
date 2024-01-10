using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsConsentManager : MonoBehaviour
{
    public GameObject consentWindow;

    private const string IS_CONSENT_GIVEN = "isConsentGiven";
    private const string FIRST_TIME = "firstTime";

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
        int consentInt = PlayerPrefs.GetInt(IS_CONSENT_GIVEN);
        Debug.Log("First time: " + LoadFirstTimeOnThisMachine());

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
        PlayerPrefs.SetInt(IS_CONSENT_GIVEN, 1);
        PlayerPrefs.Save();
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt(IS_CONSENT_GIVEN));
        ShowConsentWindow(false);
    }

    public void OptOut()
    {
        if(PlayerPrefs.GetInt(IS_CONSENT_GIVEN) == 1)
        {
            AnalyticsService.Instance.StopDataCollection();
        }
        
        PlayerPrefs.SetInt(IS_CONSENT_GIVEN, 0);
        PlayerPrefs.Save();
        
        Debug.Log("isConsentGiven: " + PlayerPrefs.GetInt(IS_CONSENT_GIVEN));
        ShowConsentWindow(false);
    }

    public void ShowConsentWindow(bool show)
    {
        consentWindow.SetActive(show);
    }

    private void SetFirstTimeOnThisMachine()
    {
        PlayerPrefs.SetInt(FIRST_TIME, 1);
    }

    private int LoadFirstTimeOnThisMachine()
    {
        return PlayerPrefs.GetInt(FIRST_TIME);
    }
}
