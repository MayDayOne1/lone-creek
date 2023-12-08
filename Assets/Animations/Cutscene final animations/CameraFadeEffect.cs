using System.Collections;
using UnityEngine;

public class CameraFadeEffect : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 2.0f;

    [Header("Background Color")]
    [ColorUsage(true, true)] // Dodany atrybut ColorUsage
    public Color backgroundColor = Color.black;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Poczekaj chwilê przed rozpoczêciem efektu fade
        yield return new WaitForSeconds(1.0f);

        // SprawdŸ, czy Camera.main i backgroundColor nie s¹ równi null
        if (Camera.main != null && backgroundColor != null)
        {
            // Ustaw kolor kamery
            Camera.main.backgroundColor = backgroundColor;

            // Efekt fade-in
            LeanTween.value(gameObject, 0.0f, 1.0f, fadeDuration)
                .setOnUpdate((float value) =>
                {
                    // Aktualizacja koloru kamery podczas efektu fade
                    Camera.main.backgroundColor = Color.Lerp(backgroundColor, Color.clear, value);
                })
                .setOnComplete(() =>
                {
                    // Efekt fade zakoñczony
                    Debug.Log("Fade-in completed!");
                });
        }
        else
        {
            // Wyœwietl komunikat b³êdu w przypadku null
            Debug.LogError("Camera.main or backgroundColor is null. Please make sure the main camera is tagged as 'MainCamera' and a valid color is assigned.");
        }
    }
}
