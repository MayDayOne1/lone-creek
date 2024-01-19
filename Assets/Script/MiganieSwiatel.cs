using UnityEngine;

public class MiganieSwiatel : MonoBehaviour
{
    public Light swiatlo; // Przypisz światło do kontrolowania

    public float amplituda = 1.0f; // Amplituda migania
    public float natężenie = 1.0f; // Natężenie światła
    public float czestotliwosc = 1.0f; // Częstotliwość migania

    private float czasPoczatkowy;

    void Start()
    {
        czasPoczatkowy = Time.time;
    }

    void Update()
    {
        // Oblicz wartość migania na podstawie amplitudy, natężenia i czasu
        float miganie = natężenie * Mathf.Sin(czestotliwosc * (Time.time - czasPoczatkowy));

        // Ustaw natężenie światła zgodnie z wartością migania
        swiatlo.intensity = miganie * amplituda;
    }
}
