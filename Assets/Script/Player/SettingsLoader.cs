using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private SettingsMenu settingsMenu;

    void Start()
    {
        settingsMenu.LoadSoundVolume();
        settingsMenu.LoadSensitivity();
    }
}
