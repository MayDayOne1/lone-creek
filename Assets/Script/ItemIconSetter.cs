using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemIconSetter : MonoBehaviour
{
    public void SetIconVisibility(float alpha)
    {
        Image[] images = GetComponentsInChildren<Image>();
        if(isActiveAndEnabled && images != null)
        {
            foreach (Image img in images)
            {
                if (img != null)
                {
                    if (!img.gameObject.CompareTag("RedFilter"))
                    {
                        img.DOFade(alpha, .1f);
                    }
                    else
                    {
                        img.DOFade(0f, .1f);
                    }
                }
            }
        }

        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.DOFade(alpha, .1f);
        }
    }

    private void Start()
    {
        SetIconVisibility(0f);
    }
}
