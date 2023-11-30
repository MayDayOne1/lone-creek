using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    public GameObject ThrowableBG;
    public GameObject AmmoBG;

    public bool IsThrowableSelected = false;
    public bool IsPrimarySelected = false;

    private PlayerController controller;
    private PlayerAnimManager animManager;
    private PlayerShootingManager shootingManager;
    private PlayerCamManager camManager;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        animManager = GetComponent<PlayerAnimManager>();
        shootingManager = GetComponent<PlayerShootingManager>();
        camManager = GetComponent<PlayerCamManager>();

        ThrowableBG.SetActive(false);
        AmmoBG.SetActive(false);
    }
    public void SelectNone()
    {
        shootingManager.SetupSelectNone();
        if(controller.IsCrouching)
        {
            camManager.ActivateCrouch();
        }
        else
        {
            camManager.ActivateNormal();
        }
        IsThrowableSelected = false;
        IsPrimarySelected = false;
        AmmoBG.SetActive(false);
        ThrowableBG.SetActive(false);

    }

    public void SelectThrowable()
    {
        SelectNone();
        animManager.SetPistol(false, controller.IsCrouching);
        if (PlayerInteract.hasThrowable)
        {
            IsThrowableSelected = true;
            shootingManager.playerBottle.SetActive(true);
            shootingManager.pistol.SetActive(false);
            AmmoBG.SetActive(false);
            ThrowableBG.SetActive(true);
        } else
        {
            SelectNone();
        }
    }
    public void SelectPrimary()
    {
        SelectNone();
        if (PlayerInteract.hasPrimary)
        {
            IsPrimarySelected = true;
            shootingManager.playerBottle.SetActive(false);
            shootingManager.pistol.SetActive(true);
            AmmoBG.SetActive(true);
            ThrowableBG.SetActive(false);
            animManager.SetPistol(true, controller.IsCrouching);
        }
    }
}