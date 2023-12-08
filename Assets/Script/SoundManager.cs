using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Utw�rz statyczn� zmienn� przechowuj�c� g�o�no��
    public static float GlobalVolume = 1.0f;

    [SerializeField] Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        // Ustaw g�o�no�� globaln� i dla wszystkich obiekt�w AudioListener w grze
        GlobalVolume = volumeSlider.value;
        AudioListener.volume = GlobalVolume;

        Save();
    }

    private void Load()
    {
        // Za�aduj g�o�no�� globaln�
        GlobalVolume = PlayerPrefs.GetFloat("musicVolume");
        volumeSlider.value = GlobalVolume;
    }

    private void Save()
    {
        // Zapisz g�o�no�� globaln�
        PlayerPrefs.SetFloat("musicVolume", GlobalVolume);
    }
}
