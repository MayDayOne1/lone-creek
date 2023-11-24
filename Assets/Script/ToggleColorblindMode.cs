using UnityEngine;
using UnityEngine.UI;

public class ToggleColorblindMode : MonoBehaviour
{
    private Toggle colorblindToggle;

    private void Start()
    {
        // Pobierz komponent Toggle na podstawie nazwy przycisku
        colorblindToggle = GameObject.Find("ColorblindToggle").GetComponent<Toggle>();

        if (colorblindToggle != null)
        {
            // Ustaw stan Toggle na obecny stan trybu dla daltonistów
            colorblindToggle.isOn = IsColorblindModeEnabled();
        }
        else
        {
            Debug.LogError("ColorblindToggle not found in the scene.");
        }
    }

    // Ta metoda jest wywo³ywana, gdy zmieni siê stan Toggle
    public void OnToggleColorblindMode(bool isColorblindMode)
    {
        // Zapisz stan trybu dla daltonistów
        PlayerPrefs.SetInt("ColorblindMode", isColorblindMode ? 1 : 0);
        PlayerPrefs.Save();

        // Tutaj mo¿esz dodaæ kod, który zmienia wygl¹d gry w zale¿noœci od trybu dla daltonistów
        // Na przyk³ad, zmiana kolorów, tekstur itp.
    }

    private bool IsColorblindModeEnabled()
    {
        // Pobierz zapisany stan trybu dla daltonistów (0 - wy³¹czony, 1 - w³¹czony)
        return PlayerPrefs.GetInt("ColorblindMode", 0) == 1;
    }
}
