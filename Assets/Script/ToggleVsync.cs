using UnityEngine;
using UnityEngine.UI;

public class ToggleVsync : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();

        if (toggle != null)
        {
            toggle.isOn = QualitySettings.vSyncCount > 0;

            // Dodaj s³uchacza tylko, jeœli skrypt jest aktywny
            if (isActiveAndEnabled)
            {
                toggle.onValueChanged.AddListener(ToggleVsyncCallback);
            }
        }
        else
        {
            Debug.LogError("Toggle component not found on the ToggleVsync object.");
        }
    }

    private void ToggleVsyncCallback(bool isVsyncEnabled)
    {
        QualitySettings.vSyncCount = isVsyncEnabled ? 1 : 0;
        Debug.Log("Vsync is now " + (QualitySettings.vSyncCount > 0 ? "enabled" : "disabled"));
    }
}
