using UnityEngine;

public class BlinkingLights : MonoBehaviour
{
    public Light lightSource;

    public float amplitude = 1.0f; 
    public float intensity = 1.0f; 
    public float frequency = 1.0f; 

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float blinking = intensity * Mathf.Sin(frequency * (Time.time - startTime));
        lightSource.intensity = blinking * amplitude;
    }
}
