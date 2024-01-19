using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientManager : MonoBehaviour
{
    [SerializeField] private AudioClip ambientMenu;
    [SerializeField] private AudioClip ambientTunnel;
    [SerializeField] private AudioClip ambientDesert;

    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetClip();
        audioSource.Play();
    }

    public void PauseClip()
    {
        audioSource.Pause();
    }

    public void ResumeClip()
    {
        audioSource.UnPause();
    }

    private void SetClip()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (sceneIndex)
        {
            case 0:
                audioSource.clip = ambientMenu;
                break;

            case 1:
                audioSource.clip = ambientTunnel;
                break;

            case 2:
                audioSource.clip = ambientDesert;
                break;
        }
    }
}
