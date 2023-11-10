using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAmmoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ClipUI;
    [SerializeField] private TextMeshProUGUI TotalAmmoUI;
    private PlayerShootingManager shootingManager;
    private readonly int maxAmmo = 24;
    private readonly int clipCapacity = 8;
    public static int currentAmmo = 0;
    public static int currentClip = 0;
    public static int savedAmmo;
    public static int savedClip;

    void Start()
    {
        shootingManager = GetComponent<PlayerShootingManager>();
        ClipUI.text = currentClip.ToString();
        TotalAmmoUI.text = currentAmmo.ToString();
    }

    public void DecrementClip()
    {
        currentClip--;
        ClipUI.text = currentClip.ToString();
    }
    public bool CanAcceptAmmo() => currentAmmo + currentClip < maxAmmo - 1;
    public void CalculateAmmoFromPickup(GameObject obj, int ammoPickup)
    {
        int ammoDiff = maxAmmo - currentAmmo;

        // case #1: ammoPickup has less or the same amount of ammo than you need
        if (ammoPickup <= ammoDiff)
        {
            currentAmmo += ammoPickup;
            TotalAmmoUI.text = currentAmmo.ToString();
            Destroy(obj);
        }
        // case #2: ammoPickup has more ammo than you can have
        else if (ammoPickup + currentAmmo > maxAmmo)
        {
            ammoDiff = ammoPickup + currentAmmo - maxAmmo;
            currentAmmo = maxAmmo;
            TotalAmmoUI.text = maxAmmo.ToString();
            obj.GetComponentInChildren<TextMeshProUGUI>().text = (ammoDiff).ToString();

        }
        if (currentClip < 1 && currentAmmo > 0) Reload();
    }
    public bool HasAmmoToShoot() => currentClip > 0;
    public void Reload()
    {
        if ((currentAmmo < 1 && currentClip < 1) ||
            (currentClip >= 8 || currentAmmo < 1))
        {
            return;
        }
        else if (currentClip < 8)
        {
            int ammoDiff = clipCapacity - currentClip;
            int ammoToReload = currentAmmo < ammoDiff ? currentAmmo : ammoDiff;
            currentClip += ammoToReload;
            currentAmmo -= ammoToReload;
            ClipUI.text = currentClip.ToString();
            TotalAmmoUI.text = currentAmmo.ToString();
        }
    }
}