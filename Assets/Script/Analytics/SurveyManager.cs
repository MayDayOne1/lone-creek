using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyManager : MonoBehaviour
{
    public static bool isActive = false;

    void Start()
    {
        if(isActive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenSurvey()
    {
        Application.OpenURL("https://forms.gle/6cGvBMVgQr9k9fDa9");
    }
}
