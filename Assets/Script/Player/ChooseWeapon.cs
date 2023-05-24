using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    private PlayerInteract playerInteract;
    public enum WEAPONS
    {
        NONE,
        THROWABLE,
        PRIMARY
    }
    public bool hasThrowable;
    public bool hasPistol;
    public WEAPONS weaponSelected;

    private void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
    }

    public void SelectThrowable()
    {
        if(hasThrowable)
        {
            weaponSelected = WEAPONS.THROWABLE;
            playerInteract.Throwable.SetActive(true);
            playerInteract.Pistol.SetActive(false);
            Debug.Log("Throwable selected");
        } else
        {
            weaponSelected = WEAPONS.NONE;
        }
        
    }

    public void SelectPrimary()
    {
        if(hasPistol)
        {
            weaponSelected = WEAPONS.PRIMARY;
            playerInteract.Throwable.SetActive(false);
            playerInteract.Pistol.SetActive(true);
            Debug.Log("Primary selected");
        } else
        {
            weaponSelected = WEAPONS.NONE;
        }
    }
}
