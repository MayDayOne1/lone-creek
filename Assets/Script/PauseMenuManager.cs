using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public PlayerController playerController;
    public void OnResume()
    {
        playerController.TogglePauseMenu();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnTryAgain()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
}
