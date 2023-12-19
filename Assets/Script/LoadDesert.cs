using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Zenject;


#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class LoadDesert : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] private AnalyticsManager analyticsManager;
#endif

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.StopCoroutine(controller.level1coroutine);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            analyticsManager.SendLevel1Completed();
#endif

            asyncLoader.LoadLevel(2);
        }
    }
}
