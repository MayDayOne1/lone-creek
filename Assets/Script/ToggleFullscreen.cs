using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    private Toggle fullscreenToggle;

    private void Start()
    {
        // Pobierz komponent Toggle na podstawie nazwy przycisku
        fullscreenToggle = GameObject.Find("Fullscreen").GetComponent<Toggle>();

        if (fullscreenToggle != null)
        {
            // Ustaw stan Toggle na obecny stan pe�noekranowy
            fullscreenToggle.isOn = Screen.fullScreen;
        }
        else
        {
            Debug.LogError("Fullscreen not found in the scene.");
        }
    }

    // Ta metoda jest wywo�ywana, gdy zmieni si� stan Toggle
    public void ToggleFullscreenMode(bool isFullscreen)
    {
        // Zmiana trybu ekranu na podstawie stanu Toggle
        Screen.fullScreen = isFullscreen;
    }
}
