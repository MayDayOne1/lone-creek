using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
	public void SetFullscreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
		SaveToPlayerPrefs("isFullScreen", isFullScreen);
	}

	public void SetCameraShake(bool isSet)
	{
		SaveToPlayerPrefs("isCamShakeEnabled", isSet);
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
}
