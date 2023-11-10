using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    private PlayerInteract playerInteract;
    private PlayerController playerController;
    private PlayerAnimManager animManager;
    
    public GameObject AmmoBG;
    public GameObject ThrowableBG;

    public bool IsThrowableSelected = false;
    public bool IsPrimarySelected = false;

    private void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
        playerController = GetComponent<PlayerController>();
        animManager = GetComponent<PlayerAnimManager>();

        AmmoBG.SetActive(false);
        ThrowableBG.SetActive(false);
    }

    public void SelectNone()
    {
        IsThrowableSelected = false;
        IsPrimarySelected = false;
        playerInteract.Throwable.SetActive(false);
        playerInteract.Pistol.SetActive(false);
        AmmoBG.SetActive(false);
        
    }
    public void SelectThrowable()
    {
        SelectNone();
        animManager.SetPistol(false, playerController.IsCrouching);
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

            animManager.SetPistol(true, playerController.IsCrouching);
        }
    }
}