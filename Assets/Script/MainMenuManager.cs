using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject onboarding;

    private void Start()
    {
        mainMenu.SetActive(true);
        onboarding.SetActive(false);
    }

    public void ShowOnboarding()
    {
        mainMenu.SetActive(false);
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
}
