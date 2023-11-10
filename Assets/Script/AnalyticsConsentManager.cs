using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsConsentManager : MonoBehaviour
{
    public GameObject consentWindow;
    public static bool? isConsentGiven = null;
    // Start is called before the first frame update
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

        //int consentInt = PlayerPrefs.GetInt("isConsentGiven");
        //if (consentInt == 0) isConsentGiven = false;
        //else if (consentInt == 1) isConsentGiven = true;
        //else isConsentGiven = null;

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
       // PlayerPrefs.SetInt("isConsentGiven", 1);
       // PlayerPrefs.Save();
        ShowConsentWindow(false);
    }

    public void OptOut()
    {
        AnalyticsService.Instance.StopDataCollection();
        isConsentGiven = false;
       // PlayerPrefs.SetInt("isConsentGiven", 0);
       // PlayerPrefs.Save();
        ShowConsentWindow(false);
    }

    private void ShowConsentWindow(bool show)
    {
        consentWindow.SetActive(show);
    }
}
