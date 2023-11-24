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
            // Ustaw stan Toggle na obecny stan trybu dla daltonist�w
            colorblindToggle.isOn = IsColorblindModeEnabled();
        }
        else
        {
            Debug.LogError("ColorblindToggle not found in the scene.");
        }
    }

    // Ta metoda jest wywo�ywana, gdy zmieni si� stan Toggle
    public void OnToggleColorblindMode(bool isColorblindMode)
    {
        // Zapisz stan trybu dla daltonist�w
        PlayerPrefs.SetInt("ColorblindMode", isColorblindMode ? 1 : 0);
        PlayerPrefs.Save();

        // Tutaj mo�esz doda� kod, kt�ry zmienia wygl�d gry w zale�no�ci od trybu dla daltonist�w
        // Na przyk�ad, zmiana kolor�w, tekstur itp.
    }

    private bool IsColorblindModeEnabled()
    {
        // Pobierz zapisany stan trybu dla daltonist�w (0 - wy��czony, 1 - w��czony)
        return PlayerPrefs.GetInt("ColorblindMode", 0) == 1;
    }
}
