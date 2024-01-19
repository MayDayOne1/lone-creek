using UnityEngine;

public class BlinkingLights : MonoBehaviour
{
    public Light lightSource; // Assign the light you want to control

    public float amplitude = 1.0f; // Amplitude of the blinking
    public float intensity = 1.0f; // Light intensity
    public float frequency = 1.0f; // Blinking frequency

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the blinking value based on amplitude, intensity, and time
        float blinking = intensity * Mathf.Sin(frequency * (Time.time - startTime));

        // Set the light intensity based on the blinking value
        lightSource.intensity = blinking * amplitude;
    }
}
