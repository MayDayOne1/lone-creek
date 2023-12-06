using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Utwórz statyczn¹ zmienn¹ przechowuj¹c¹ g³oœnoœæ
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
        // Ustaw g³oœnoœæ globaln¹ i dla wszystkich obiektów AudioListener w grze
        GlobalVolume = volumeSlider.value;
        AudioListener.volume = GlobalVolume;

        Save();
    }

    private void Load()
    {
        // Za³aduj g³oœnoœæ globaln¹
        GlobalVolume = PlayerPrefs.GetFloat("musicVolume");
        volumeSlider.value = GlobalVolume;
    }

    private void Save()
    {
        // Zapisz g³oœnoœæ globaln¹
        PlayerPrefs.SetFloat("musicVolume", GlobalVolume);
    }
}
