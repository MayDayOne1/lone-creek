using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Zenject;


#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class TriggerVictory : MonoBehaviour
{
    public GameObject VictoryScreen;
    public PlayerController controller;
    [SerializeField] private ASyncLoader asyncLoader;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] AnalyticsManager analyticsManager;
#endif

    private void Start()
    {
        VictoryScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            VictoryScreen.SetActive(true);
            controller.VictorySetup();

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            analyticsManager.SendLevel2Completed();
#endif

        }
    }

    public void OnReturnToMainMenu()
    {
        asyncLoader.LoadLevel(0);
        SurveyManager.isActive = true;
    }
}
