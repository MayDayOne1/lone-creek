using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public PlayerController playerController;
    public void OnResume()
    {
        playerController.TogglePauseMenu();
    }

    public void OnQuit()
    {
        
    }
}
