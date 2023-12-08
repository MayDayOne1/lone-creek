using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    public ThrowableWeapon throwableWeapon;
    public PistolWeapon pistolWeapon;

    private PlayerShootingManager shootingManager;

    private void Start()
    {
        shootingManager = GetComponent<PlayerShootingManager>();
    }

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