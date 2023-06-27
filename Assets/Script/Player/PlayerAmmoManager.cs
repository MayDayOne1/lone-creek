using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAmmoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ClipUI;
    [SerializeField] private TextMeshProUGUI TotalAmmoUI;
    private PlayerShootingManager shootingManager;
    private int maxAmmo = 32;
    private int currentAmmo = 0;
    private int clipCapacity = 8;
    private int currentClip;

    void Start()
    {
        shootingManager = GetComponent<PlayerShootingManager>();
        currentClip = 0;
        ClipUI.text = currentClip.ToString();
        TotalAmmoUI.text = currentAmmo.ToString();
    }

    public void DecrementClip()
    {
        currentClip--;
        ClipUI.text = currentClip.ToString();
    }
    public bool CanAcceptAmmo() => !(currentAmmo + currentClip > maxAmmo - 1);
    public void SetAmmo(int ammo)
    {
        if (ammo <= currentClip)
        {
            currentClip = ammo;
        }
        else
        {
            currentClip = clipCapacity;
            currentAmmo = ammo - currentClip;
        }
        ClipUI.text = currentClip.ToString();
        TotalAmmoUI.text = currentAmmo.ToString();
    }
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
        if (currentClip < 1) Reload();
    }
    public bool HasAmmoToShoot() => currentClip > 0;
    public void Reload()
    {
        if (currentAmmo < 1 && currentClip < 1)
        {
            shootingManager.DisablePistol();
            return;
        }
        else if (currentClip >= 8 || currentAmmo < 1)
        {
            return;
        }
        else if (currentClip < 8)
        {
            int ammoDiff = clipCapacity - currentClip;
            int ammoToReload = currentAmmo < ammoDiff ? ammoToReload = currentAmmo : ammoToReload = ammoDiff;
            currentClip += ammoToReload;
            currentAmmo -= ammoToReload;
            ClipUI.text = currentClip.ToString();
            TotalAmmoUI.text = currentAmmo.ToString();
        }
    }
}
