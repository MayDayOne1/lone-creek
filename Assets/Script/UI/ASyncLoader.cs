using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image loadingCircle;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider; // Zmieni³em GameObject na Slider

    private void Start()
    {
        loadingScreen.SetActive(false);
    }

    public void LoadLevel(int levelIndex)
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelIndex));
    }

    IEnumerator LoadLevelASync(int levelIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelIndex);
        // Debug.Log("Progress: " + loadOperation.progress.ToString());
        while (!loadOperation.isDone) // Poprawi³em LoadOperation na loadOperation
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            // loadingCircle.transform.Rotate(0, 0, 60f);
            yield return null;
        }
    }
}