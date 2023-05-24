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
        PRIMARY,
        SECONDARY
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
            // Debug.Log("Throwable selected");
        } else
        {
            weaponSelected = WEAPONS.NONE;
        }
        
    }

    public void SelectPrimary()
    {
        weaponSelected = WEAPONS.PRIMARY;
        // Debug.Log("Primary selected");
    }
}
