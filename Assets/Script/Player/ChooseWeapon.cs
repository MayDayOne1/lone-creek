using UnityEngine;

public class ChooseWeapon : MonoBehaviour
{
    private PlayerInteract playerInteract;
    private PlayerController playerController;
    private Animator animator;
    public enum WEAPONS
    {
        NONE,
        THROWABLE,
        PRIMARY
    }
    public WEAPONS weaponSelected;
    public bool hasThrowable;
    public bool hasPistol;
    public GameObject AmmoBG;
    public GameObject ThrowableBG;

    private void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        AmmoBG.SetActive(false);
        ThrowableBG.SetActive(false);
    }

    public void SelectNone()
    {
        weaponSelected = WEAPONS.NONE;
        playerInteract.Throwable.SetActive(false);
        playerInteract.Pistol.SetActive(false);
        AmmoBG.SetActive(false);
        animator.SetLayerWeight(3, 0);
        animator.SetLayerWeight(4, 0);
    }

    public void SelectThrowable()
    {
        animator.SetLayerWeight(3, 0);
        animator.SetLayerWeight(4, 0);
        if (hasThrowable)
        {
            weaponSelected = WEAPONS.THROWABLE;
            playerInteract.Throwable.SetActive(true);
            playerInteract.Pistol.SetActive(false);
            // Debug.Log("Throwable selected");
            AmmoBG.SetActive(false);
            ThrowableBG.SetActive(true);
        } else
        {
            SelectNone();
        }
        
    }
    public void SelectPrimary()
    {
        if(hasPistol)
        {
            weaponSelected = WEAPONS.PRIMARY;
            playerInteract.Throwable.SetActive(false);
            playerInteract.Pistol.SetActive(true);
            AmmoBG.SetActive(true);
            ThrowableBG.SetActive(false);

            if(playerController.IsCrouching)
            {
                animator.SetLayerWeight(4, 1);
                animator.SetLayerWeight(3, 0);
            } else
            {
                animator.SetLayerWeight(4, 0);
                animator.SetLayerWeight(3, 1);
            }
            // Debug.Log("Primary selected");
        } else
        {
            SelectNone();
        }
    }
}