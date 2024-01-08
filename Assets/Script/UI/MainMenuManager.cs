using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using MEC;
using Zenject;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settings;
    [SerializeField] private GameObject onboarding;
    [SerializeField] private SettingsMenu settingsMenu;
    private bool isViewingOnboarding = false;

    [Inject] private PlayerController controller;
    [Inject] private AnalyticsManager analyticsManager;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public float onboardingTimeSpent = 0f;
#endif

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        onboarding.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 1f;

        settingsMenu.LoadAllSettings();
    }

    public void ShowOnboarding()
    {
        mainMenu.gameObject.SetActive(false);
        onboarding.SetActive(true);
        isViewingOnboarding = true;
        Timing.RunCoroutine(CountOnboardingTime());
    }

    private IEnumerator<float> CountOnboardingTime()
    {
        while(isViewingOnboarding)
        {
            onboardingTimeSpent += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    public void StartGame()
    {
        isViewingOnboarding = false;
        StopCoroutine(CountOnboardingTime());

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        controller.ResetAnalyticsData();
        analyticsManager.SendOnboardingCompleted(onboardingTimeSpent);
#endif

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
