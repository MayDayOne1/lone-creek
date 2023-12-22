using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Zenject;
using UnityEngine.Video;
using MEC;




#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class TriggerVictory : MonoBehaviour
{
    public GameObject VictoryScreen;
    public PlayerController controller;
    [SerializeField] private ASyncLoader asyncLoader;
    [SerializeField] private GameObject[] objectsToDestroy;

    private VideoPlayer videoPlayer;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] private AnalyticsManager analyticsManager;
#endif

    private void Start()
    {
        VictoryScreen.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // VictoryScreen.SetActive(true);
            controller.VictorySetup();
            Timing.RunCoroutine(PlayOutroCoroutine().CancelWith(gameObject));

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            analyticsManager.SendLevel2Completed(controller.level2TimeSpent);
#endif

        }
    }

    public void OnReturnToMainMenu()
    {
        asyncLoader.LoadLevel(0);
        SurveyManager.isActive = true;
    }

    private IEnumerator<float> PlayOutroCoroutine()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        videoPlayer.Play();
        yield return Timing.WaitForSeconds(31.5f);
        OnReturnToMainMenu();
    }
}
