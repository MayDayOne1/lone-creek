using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    public PlayerInteract playerInteract;
    public ChooseWeapon chooseWeapon;
    public PlayerAim playerAim;
    public Rigidbody PlayerBottle;

    public float ThrowForce = 10f;

    public void Shoot()
    {
        if(playerAim.IsAiming)
        {
            if (chooseWeapon.weaponSelected == ChooseWeapon.WEAPONS.THROWABLE)
            {
                PlayerBottle.isKinematic = false;
                PlayerBottle.AddForce(cam.transform.forward * ThrowForce, ForceMode.VelocityChange);
            }
        }
        
    }
}
