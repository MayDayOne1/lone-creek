using UnityEngine;
using Zenject;

public class ChooseWeapon : MonoBehaviour
{
    public ThrowableWeapon throwableWeapon;
    public PistolWeapon pistolWeapon;

    [Inject] PlayerShootingManager shootingManager;

    public void SelectThrowable()
    {
        shootingManager.currentWeapon?.Disable();
        if(PlayerInteract.hasThrowable)
        {
            shootingManager.currentWeapon = throwableWeapon;
            shootingManager.currentWeapon.Select();
        }
        
    }
    public void SelectPrimary()
    {
        shootingManager.currentWeapon?.Disable();
        if(PlayerInteract.hasPrimary)
        {
            shootingManager.currentWeapon = pistolWeapon;
            shootingManager.currentWeapon.Select();
        }
    }
}