using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settings;
    [SerializeField] private GameObject onboarding;

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
    }

    public void StartGame()
    {
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
        StartCoroutine(TweenWindow(settings));
    }
    public void HideSettings()
    {
        ShowCanvasGroup(settings, false);
        StartCoroutine(TweenWindow(mainMenu));
        settings.gameObject.SetActive(false);
    }

    private IEnumerator TweenWindow(CanvasGroup group)
    {
        yield return new WaitForSeconds(.2f);
        ShowCanvasGroup(group, true);
    }

    private void ShowCanvasGroup(CanvasGroup group, bool show)
    {
        group.interactable = show;
        group.blocksRaycasts = show;
        if (show)
        {
            group.DOFade(1f, .1f);
        }
        else
        {
            group.DOFade(0f, .1f);
        }
    }
}
