using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    private PlayerInteract playerInteract;
    private Animator animator;
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
        animator = GetComponent<Animator>();
    }

    public void SelectThrowable()
    {
        animator.SetLayerWeight(3, 0);
        if (hasThrowable)
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
            animator.SetLayerWeight(3, 1);
            Debug.Log("Primary selected");
        } else
        {
            animator.SetLayerWeight(3, 0);
            weaponSelected = WEAPONS.NONE;
        }
    }
}
