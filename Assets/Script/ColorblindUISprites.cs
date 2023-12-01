using UnityEngine;
using UnityEngine.UI;

public class ColorblindUISprites : MonoBehaviour
{
    public Sprite protanopiaImage;
    private Image myIMGcomponent;

    public Sprite deuteranopiaImage;
    private Image myIMGcomponent2;

    void Start()
    {
        if(PlayerPrefs.GetInt("ToggleBool2") == 1)
        {
            myIMGcomponent = this.GetComponent<Image>();
            myIMGcomponent.sprite = protanopiaImage;
        }

        if(PlayerPrefs.GetInt("ToggleBool3") == 1)
        {
            myIMGcomponent2 = this.GetComponent<Image>();
            myIMGcomponent2.sprite = deuteranopiaImage;
        }
    }
}
