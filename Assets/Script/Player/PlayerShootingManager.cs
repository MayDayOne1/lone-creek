using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static ChooseWeapon;

public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private Transform PlayerBottle;
    [SerializeField] private GameObject ThrowablePlayerBottle;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float ThrowStrength = 20f;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;

    private ChooseWeapon chooseWeapon;
    private Animator animator;
    private PlayerInteract playerInteract;
    private GameObject BottleToInstantiate;
    public Camera cam;

    public bool IsAimingThrowable = false;
    public bool IsAimingPistol = false;

    [Header("Pistol")]
    private int maxAmmo = 24;
    private int currentAmmo;
    private int clipCapacity = 8;
    private int currentClip;


    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animator = GetComponent<Animator>();
        playerInteract = GetComponent<PlayerInteract>();

        currentClip = clipCapacity;
        currentAmmo = maxAmmo - currentClip;
    }

    private void OnEnable()
    {
        aimAction.action.Enable();
    }

    private void OnDisable()
    {
        aimAction.action.Disable();
    }

    public void Aim()
    {
        float aimValue = aimAction.action.ReadValue<float>();
        // Debug.Log("Aim value " + aimValue);
        if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
        {
            if(aimValue == 1f)
            {
                IsAimingThrowable = true;
                animator.SetLayerWeight(2, 1);
                animator.SetBool("isAimingThrowable", true);
                DrawLine();
                
            } else
            {
                IsAimingThrowable = false;
                animator.SetLayerWeight(2, 0);
                lineRenderer.enabled = false;
                animator.SetBool("isAimingThrowable", false);
            }
        } else if (chooseWeapon.weaponSelected == WEAPONS.PRIMARY)
        {
            if(aimValue == 1f)
            {
                IsAimingPistol = true;
            } else
            {
                IsAimingPistol = false;
            }
        }        
    }

    public void Shoot()
    {
        if(IsAimingThrowable && playerInteract.Throwable.activeSelf)
        {
            animator.SetTrigger("Throw");
            playerInteract.Throwable.SetActive(false);
            PlayerBottle.gameObject.SetActive(false);
            chooseWeapon.weaponSelected = WEAPONS.NONE;
            Transform instancePos = PlayerBottle.transform;
            BottleToInstantiate = Instantiate(ThrowablePlayerBottle, instancePos.position, instancePos.rotation);
            Rigidbody bottleRb = BottleToInstantiate.GetComponent<Rigidbody>();
            bottleRb.AddForce(cam.transform.forward * ThrowStrength, ForceMode.VelocityChange);
            Destroy(BottleToInstantiate, 2f);
            chooseWeapon.hasThrowable = false;
            IsAimingThrowable = false;
        } else if (IsAimingPistol && playerInteract.Pistol.activeSelf)
        {
            animator.SetTrigger("Shoot");
            ShootPistol();
        }
    }
    private void DrawLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints + 1);
        Vector3 startPos = PlayerBottle.position;
        Vector3 startVelocity = ThrowStrength * cam.transform.forward;
        // Debug.Log("Cam transform position: " + Cam.transform.position);
        int i = 0;
        lineRenderer.SetPosition(i, startPos);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);
        }
    }

    private void ShootPistol()
    {
        currentClip--;
        if(currentClip <= 0)
        {
            Reload();
        }
        if(currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                Debug.Log("Shot something!");
                Debug.Log("Current clip: " + currentClip);
                Debug.Log("Current ammo: " + currentAmmo);
            }
        }
        
    }

    private void Reload()
    {
        if(currentAmmo >= 8)
        {
            currentClip = clipCapacity;
            currentAmmo -= currentClip;
        } else
        {
            currentClip = currentAmmo;
            currentAmmo = 0;
        }
        Debug.Log("Reloading!");
    }
}
