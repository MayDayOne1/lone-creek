using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class PlayerEnterCabin : MonoBehaviour
{
    private bool hasEntered = false;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] AnalyticsManager analyticsManager;
#endif

    void OnTriggerEnter(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if(controller != null && !hasEntered)
        {
            hasEntered = true;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            analyticsManager.SendPlayerReachedCabin(controller.level2TimeSpent);
#endif
        }
    }
}
