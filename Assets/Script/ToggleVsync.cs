using UnityEngine;
using UnityEngine.UI;

public class ToggleVsync : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        // Pobierz komponent Toggle w skrypcie
        toggle = GetComponent<Toggle>();

        // SprawdŸ, czy komponent Toggle zosta³ poprawnie znaleziony
        if (toggle != null)
        {
            // Ustaw stan Toggle na obecny stan Vsync
            toggle.isOn = QualitySettings.vSyncCount > 0;

            // Dodaj obiekt nas³uchuj¹cy na zmianê stanu Toggle
            toggle.onValueChanged.AddListener(ToggleVsyncCallback);
        }
        else
        {
            Debug.LogError("Toggle component not found on the ToggleVsync object.");
        }
    }

    private void ToggleVsyncCallback(bool isVsyncEnabled)
    {
        // Ustaw Vsync w zale¿noœci od stanu Toggle
        QualitySettings.vSyncCount = isVsyncEnabled ? 1 : 0;

        // Wyœwietl informacjê w konsoli po zmianie stanu Vsync
        Debug.Log("Vsync is now " + (QualitySettings.vSyncCount > 0 ? "enabled" : "disabled"));
    }
}
