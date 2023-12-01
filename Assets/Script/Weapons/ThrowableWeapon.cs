using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour, IWeapon
{
    public void DisableWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void Select()
    {
        throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public void StartAim()
    {
        throw new System.NotImplementedException();
    }

    public void StopAim()
    {
        throw new System.NotImplementedException();
    }
}
