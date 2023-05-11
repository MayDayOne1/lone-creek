using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    public PlayerInteract playerInteract;
    public enum WEAPONS
    {
        NONE,
        THROWABLE,
        PRIMARY,
        SECONDARY
    }

    public WEAPONS weaponSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectThrowable()
    {
        if(playerInteract.Throwable.activeSelf == true)
        {
            weaponSelected = WEAPONS.THROWABLE;
            Debug.Log("Throwable selected");
        } else
        {
            weaponSelected = WEAPONS.NONE;
        }
        
    }

    public void SelectPrimary()
    {
        weaponSelected = WEAPONS.PRIMARY;
        Debug.Log("Primary selected");
    }

    public void SelectSecondary()
    {
        weaponSelected = WEAPONS.SECONDARY;
        Debug.Log("Secondary selected");
    }
}
