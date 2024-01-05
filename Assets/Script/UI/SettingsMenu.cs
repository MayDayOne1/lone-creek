using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] Toggle vsyncToggle;
	[SerializeField] Toggle fullScreenToggle;
	[SerializeField] Toggle camShakeToggle;
	[SerializeField] Slider volumeSlider;
	[SerializeField] Slider sensitivitySliderX;
	[SerializeField] Slider sensitivitySliderY;

	private const string VSYNC_KEY = "isVsync";
	private const string FULLSCREEN_KEY = "isFullScreen";
	private const string CAM_SHAKE_KEY = "isCamShakeEnabled";
	private const string SOUND_VOLUME_KEY = "soundVolume";
	private const string SENSITIVITY_KEY_X = "sensitivityX";
	private const string SENSITIVITY_KEY_Y = "sensitivityY";

    void Start()
    {
		LoadVsync();
		LoadFullScreen();
		LoadCamShake();
		LoadSoundVolume();
		LoadSensitivity();
    }

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
		SaveToPlayerPrefs(VSYNC_KEY, isSet);
	}

	public void SetFullscreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
		SaveToPlayerPrefs(FULLSCREEN_KEY, isFullScreen);
	}

	public void SetCameraShake(bool isSet)
	{
		SaveToPlayerPrefs(CAM_SHAKE_KEY, isSet);
	}

	public void SetVolume()
	{
		AudioListener.volume = volumeSlider.value;
		SaveFloatToPlayerPrefs(SOUND_VOLUME_KEY, volumeSlider.value);
	}

	public void SetSensitivityX()
	{
		PlayerParams.sensitivityX = sensitivitySliderX.value;
		SaveFloatToPlayerPrefs(SENSITIVITY_KEY_X, sensitivitySliderX.value);
	}

	public void SetSensitivityY()
	{
		PlayerParams.sensitivityY = sensitivitySliderY.value;
		SaveFloatToPlayerPrefs(SENSITIVITY_KEY_Y, sensitivitySliderY.value);
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

	private void LoadToggleFromPlayerPrefs(Toggle toggle, string key)
	{
		int setting = PlayerPrefs.GetInt(key);
		if(setting == 1)
		{
			toggle.isOn = true;
		}
		else
		{
			toggle.isOn = false;
		}
	}

	private void LoadVsync()
	{
		LoadToggleFromPlayerPrefs(vsyncToggle, VSYNC_KEY);
		QualitySettings.vSyncCount = PlayerPrefs.GetInt(VSYNC_KEY);
	}

	private void LoadFullScreen()
	{
		LoadToggleFromPlayerPrefs(fullScreenToggle, FULLSCREEN_KEY);
		if(fullScreenToggle.isOn)
		{
			Screen.fullScreen = true;
        }
		else
		{
			Screen.fullScreen = false;
		}
	}

	private void LoadCamShake()
	{
		LoadToggleFromPlayerPrefs(camShakeToggle, CAM_SHAKE_KEY);
	}

    public void LoadSoundVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
        AudioListener.volume = volumeSlider.value;
    }

	public void LoadSensitivity()
	{
		sensitivitySliderX.value = PlayerPrefs.GetFloat(SENSITIVITY_KEY_X);
		sensitivitySliderY.value = PlayerPrefs.GetFloat(SENSITIVITY_KEY_Y);
	}
}
