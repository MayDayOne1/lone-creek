using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    private PlayerInteract playerInteract;
    private PlayerController controller;
    private PlayerAnimManager animManager;
    private PlayerShootingManager shootingManager;
    PlayerCamManager camManager;
    
    public GameObject AmmoBG;
    public GameObject ThrowableBG;

    public bool IsThrowableSelected = false;
    public bool IsPrimarySelected = false;

    private void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
        controller = GetComponent<PlayerController>();
        animManager = GetComponent<PlayerAnimManager>();
        shootingManager = GetComponent<PlayerShootingManager>();
        camManager = GetComponent<PlayerCamManager>();

        AmmoBG.SetActive(false);
        ThrowableBG.SetActive(false);
    }
    public void SelectNone()
    {
        shootingManager.SetAimRigWeight(0f);
        shootingManager.SetCrosshairVisibility(false);
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
        playerInteract.Throwable.SetActive(false);
        playerInteract.Pistol.SetActive(false);
        AmmoBG.SetActive(false);
        ThrowableBG.SetActive(false);

    }
    public void SelectThrowable()
    {
        SelectNone();
        shootingManager.SetAimRigWeight(0f);
        animManager.SetPistol(false, controller.IsCrouching);
        if (PlayerInteract.hasThrowable)
        {
            IsThrowableSelected = true;
            playerInteract.Throwable.SetActive(true);
            playerInteract.Pistol.SetActive(false);
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
            playerInteract.Throwable.SetActive(false);
            playerInteract.Pistol.SetActive(true);
            AmmoBG.SetActive(true);
            ThrowableBG.SetActive(false);
            animManager.SetPistol(true, controller.IsCrouching);
        }
    }
}