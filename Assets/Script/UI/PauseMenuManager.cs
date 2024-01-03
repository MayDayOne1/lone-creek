using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


public class PauseMenuManager : MonoBehaviour
{
    public PlayerController controller;

    [SerializeField] private Canvas pauseMenuCanvas;
    [SerializeField] private CanvasGroup settings;
    private CanvasGroup pauseMenu;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] private AnalyticsManager analyticsManager;
#endif

    private void Start()
    {
        pauseMenu = GetComponent<CanvasGroup>();
        pauseMenuCanvas.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
    }

    public void OnResume()
    {
        controller.TogglePauseMenu();
    }

    public void OnQuit()
    {

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            controller.StopCoroutine(controller.level1coroutine);
            analyticsManager.SendLevel1Quit(controller.level1TimeSpent);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            controller.StopCoroutine(controller.level2coroutine);
            analyticsManager.SendLevel2Quit(controller.level2TimeSpent);
        }
#endif

        Time.timeScale = 1f;
        SurveyManager.isActive = true;
        SceneManager.LoadScene(0);
    }

    public void OnTryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ShowSettings()
    {
        settings.gameObject.SetActive(true);

        ShowCanvasGroup(pauseMenu, false);
        ShowCanvasGroup(settings, true);
    }
    public void HideSettings()
    {
        ShowCanvasGroup(settings, false);
        ShowCanvasGroup(pauseMenu, true);
        settings.gameObject.SetActive(false);
    }

    private void ShowCanvasGroup(CanvasGroup group, bool show)
    {
        group.interactable = show;
        group.blocksRaycasts = show;
        if(show)
        {
            group.alpha = 1f;
        }
        else
        {
            group.alpha = 0f;
        }
    }
}
