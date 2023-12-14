using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] Slider volumeSlider;

	public void SetVsync(bool isSet)
	{
		if (isSet)
		{
            QualitySettings.vSyncCount = 1;
        }
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		SaveToPlayerPrefs("isVsync", isSet);
	}

	public void SetFullscreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
		SaveToPlayerPrefs("isFullScreen", isFullScreen);
	}

	public void SetCameraShake(bool isSet)
	{
		SaveToPlayerPrefs("isCamShakeEnabled", isSet);
	}

	public void SetVolume()
	{
		AudioListener.volume = volumeSlider.value;
		SaveFloatToPlayerPrefs("soundVolume", volumeSlider.value);
	}

	private void SaveToPlayerPrefs(string key, bool isSet)
	{
		if(isSet)
		{
			PlayerPrefs.SetInt(key, 1);
		}
		else
		{
			PlayerPrefs.SetInt(key, 0);
		}
		PlayerPrefs.Save();
	}

	private void SaveFloatToPlayerPrefs(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}
}
