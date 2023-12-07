using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private float currentIntensity;

    private void Start()
    {
        currentIntensity = targetLight.intensity;
        Timing.RunCoroutine(FlickerLight());
    }

    private IEnumerator<float> FlickerLight()
    {
        while (true)
        {
            // Randomize intensity within the specified range
            float randomIntensity = Random.Range(minIntensity, maxIntensity);

            // Lerp between the current intensity and the random intensity over time
            float t = 0;
            while (t < flickerSpeed)
            {
                t += Time.deltaTime;
                float normalizedTime = t / flickerSpeed;
                targetLight.intensity = Mathf.Lerp(currentIntensity, randomIntensity, normalizedTime);
                yield return Timing.WaitForOneFrame;
            }

            // Update current intensity to the random intensity for the next iteration
            currentIntensity = randomIntensity;
        }
    }
}
