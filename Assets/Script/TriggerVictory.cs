using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerVictory : MonoBehaviour
{
    public GameObject VictoryScreen;
    public PlayerController playerController;

    private void Start()
    {
        VictoryScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            VictoryScreen.SetActive(true);
            playerController.VictorySetup();
        }
    }

    public void OnReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
