using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void DisableWeapon();
    public void Select();
    public void Shoot();
    public void StartAim();
    public void StopAim();
}
