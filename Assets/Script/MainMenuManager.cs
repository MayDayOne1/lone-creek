using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settings;
    [SerializeField] private GameObject onboarding;
    private bool isViewingOnboarding = false;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public float onboardingTimeSpent = 0f;
#endif

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        onboarding.SetActive(false);
    }

    public void ShowOnboarding()
    {
        mainMenu.gameObject.SetActive(false);
        onboarding.SetActive(true);
        isViewingOnboarding = true;
        StartCoroutine(CountOnboardingTime());
    }

    private IEnumerator CountOnboardingTime()
    {
        while(isViewingOnboarding)
        {
            onboardingTimeSpent += Time.deltaTime;
            yield return null;
        }
    }

    public void StartGame()
    {
        isViewingOnboarding = false;
        StopCoroutine(CountOnboardingTime());
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsService.Instance.CustomData("onboardingCompleted", new Dictionary<string, object>()
        {
            { "onboardingTimeSpent", onboardingTimeSpent }
        });
#endif
        SceneManager.LoadScene("SceneTunnel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowSettings()
    {
        settings.gameObject.SetActive(true);
        
        ShowCanvasGroup(mainMenu, false);
        ShowCanvasGroup(settings, true);
    }
    public void HideSettings()
    {
        ShowCanvasGroup(settings, false);
        ShowCanvasGroup(mainMenu, true);
        settings.gameObject.SetActive(false);
    }

    private void ShowCanvasGroup(CanvasGroup group, bool show)
    {
        group.interactable = show;
        group.blocksRaycasts = show;
        if (show)
        {
            group.DOFade(1f, .2f);
        }
        else
        {
            group.DOFade(0f, .2f);
        }
    }
}
