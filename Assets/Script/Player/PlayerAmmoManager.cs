using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAmmoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ClipUI;
    [SerializeField] private TextMeshProUGUI TotalAmmoUI;
    private readonly int maxAmmo = 24;
    private readonly int clipCapacity = 8;

    void Start()
    {
        ClipUI.text = PlayerParams.currentClip.ToString();
        TotalAmmoUI.text = PlayerParams.currentAmmo.ToString();
    }

    public void DecrementClip()
    {
        PlayerParams.currentClip--;
        ClipUI.text =PlayerParams.currentClip.ToString();
    }
    public bool CanAcceptAmmo() => PlayerParams.currentAmmo + PlayerParams.currentClip < maxAmmo - 1;
    public void CalculateAmmoFromPickup(GameObject obj, int ammoPickup)
    {
        int ammoDiff = maxAmmo -PlayerParams.currentAmmo;

        // case #1: ammoPickup has less or the same amount of ammo than you need
        if (ammoPickup <= ammoDiff)
        {
            PlayerParams.currentAmmo += ammoPickup;
            TotalAmmoUI.text = PlayerParams.currentAmmo.ToString();
            Destroy(obj);
        }
        // case #2: ammoPickup has more ammo than you can have
        else if (ammoPickup +PlayerParams.currentAmmo > maxAmmo)
        {
            ammoDiff = ammoPickup + PlayerParams.currentAmmo - maxAmmo;
           PlayerParams.currentAmmo = maxAmmo;
            TotalAmmoUI.text = maxAmmo.ToString();
            obj.GetComponentInChildren<TextMeshProUGUI>().text = (ammoDiff).ToString();

        }
        if (PlayerParams.currentClip < 1 && PlayerParams.currentAmmo > 0) Reload();
    }
    public bool HasAmmoToShoot() => PlayerParams.currentClip > 0;
    public void Reload()
    {
        if ((PlayerParams.currentAmmo < 1 && PlayerParams.currentClip < 1) ||
        (PlayerParams.currentClip >= 8 ||PlayerParams.currentAmmo < 1))
        {
            return;
        }
        else if (PlayerParams.currentClip < 8)
        {
            int ammoDiff = clipCapacity - PlayerParams.currentClip;
            int ammoToReload = PlayerParams.currentAmmo < ammoDiff ? PlayerParams.currentAmmo : ammoDiff;
            PlayerParams.currentClip += ammoToReload;
            PlayerParams.currentAmmo -= ammoToReload;
            ClipUI.text = PlayerParams.currentClip.ToString();
            TotalAmmoUI.text = PlayerParams.currentAmmo.ToString();
        }
    }
}