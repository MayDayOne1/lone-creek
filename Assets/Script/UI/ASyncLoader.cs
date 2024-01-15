using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MEC;
using UnityEngine.Video;
using Zenject;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image loadingCircle;
    [SerializeField] private GameObject onboarding;

    [SerializeField] private GameObject[] objectsToDestroy;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [Inject] private AmbientManager ambientManager;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        loadingScreen.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex == 1)
        {
            mainMenu.SetActive(false);
            Timing.RunCoroutine(PlayIntroCoroutine(levelIndex).CancelWith(gameObject));
        } else
        {
            Timing.RunCoroutine(LoadLevelASync(levelIndex));
        }
    }

    public void Restart()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        Timing.RunCoroutine(LoadLevelASync(levelIndex));
    }

    private IEnumerator<float> LoadLevelASync(int levelIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelIndex);
        // Debug.Log("Progress: " + loadOperation.progress.ToString());
        while (!loadOperation.isDone) // Poprawi³em LoadOperation na loadOperation
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            // loadingCircle.transform.Rotate(0, 0, 60f);
            yield return Timing.WaitForOneFrame;
        }
    }

    private IEnumerator<float> PlayIntroCoroutine(int levelIndex)
    {
        ambientManager.PauseClip();
        loadingScreen.SetActive(false);
        if(onboarding != null)
        {
            onboarding.SetActive(false);
        }

        foreach(GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        
        videoPlayer.Play();
        yield return Timing.WaitForSeconds(11f);
        loadingScreen.SetActive(true);
        Timing.RunCoroutine(LoadLevelASync(levelIndex));
    }
}