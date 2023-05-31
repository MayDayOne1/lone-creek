using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ChooseWeapon;

public class PlayerShootingManager : MonoBehaviour
{
    [Header("GENERAL")]
    [SerializeField] private InputActionReference aimAction;
    private ChooseWeapon chooseWeapon;
    private Animator animator;
    private PlayerInteract playerInteract;
    public Camera cam;


    [Header("THROW")]
    [SerializeField] private Transform PlayerBottle;
    [SerializeField] private GameObject ThrowablePlayerBottle;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float ThrowStrength = 20f;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    private GameObject BottleToInstantiate;
    public bool IsAimingThrowable = false;

    [Header("PISTOL")]
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform dummyTransform;
    private int maxAmmo = 24;
    private int currentAmmo;
    private int clipCapacity = 8;
    private int currentClip;
    private float cooldown = .5f;
    private float cooldownTimer;
    private Transform hitTransform = null;
    public bool IsAimingPistol = false;
    public GameObject hitEffect;
    public ParticleSystem particles;

    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animator = GetComponent<Animator>();
        playerInteract = GetComponent<PlayerInteract>();
        particles = playerInteract.Pistol.GetComponentInChildren<ParticleSystem>();

        currentClip = clipCapacity;
        currentAmmo = maxAmmo - currentClip;
        cooldownTimer = cooldown;
    }
    #region AimActionEnableDisable
    private void OnEnable()
    {
        aimAction.action.Enable();
    }

    private void OnDisable()
    {
        aimAction.action.Disable();
    }
    #endregion
    private void AimTowardsCrosshair()
    {
        Vector3 mouseWorldPos = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            dummyTransform.position = hit.point;
            mouseWorldPos = hit.point;
            hitTransform = hit.transform;
        }
    }
    private void StartAimingThrowable()
    {
        IsAimingThrowable = true;
        animator.SetLayerWeight(2, 1);
        animator.SetBool("isAimingThrowable", true);
        DrawLine();
    }
    private void StopAimingThrowable()
    {
        IsAimingThrowable = false;
        animator.SetLayerWeight(2, 0);
        lineRenderer.enabled = false;
        animator.SetBool("isAimingThrowable", false);
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
    private void Throw()
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
    }
    private void Fire()
    {
        particles.Play();
        playerInteract.audioSource.Play();
        currentClip--;
        if (hitTransform != null)
        {
            // Debug.Log("Current clip: " + currentClip);
            GameObject hitParticles = Instantiate(hitEffect, hitTransform.position, Quaternion.identity);
            Destroy(hitParticles, 2.0f);
        }
    }
    private void DisablePistol()
    {
        playerInteract.Pistol.SetActive(false);
        animator.SetLayerWeight(3, 0);
        chooseWeapon.weaponSelected = WEAPONS.NONE;
    }
    private void ShootPistol()
    {
        if (currentClip <= 0)
        {
            Reload();
        }
        if (Time.time - cooldownTimer < cooldown)
        {
            return;
        }
        else if (currentClip > 0)
        {
            cooldownTimer = Time.time;
            Fire();
        }
        else if (currentAmmo <= 0)
        {
            DisablePistol();
        }
    }
    public void Aim()
    {
        AimTowardsCrosshair();
        float aimValue = aimAction.action.ReadValue<float>();
        // Debug.Log("Aim value " + aimValue);
        if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
        {
            if(aimValue == 1f)
            {
                StartAimingThrowable();
            } else
            {
                StopAimingThrowable();
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
            Throw();
        } else if (IsAimingPistol && playerInteract.Pistol.activeSelf)
        {
            ShootPistol();
        }
    }
    public void Reload()
    {
        if(currentClip >= 8)
        {
            return;
        }
        else if(currentAmmo >= 8)
        {
            animator.SetTrigger("Reload");
            currentClip = clipCapacity;
            currentAmmo -= currentClip;
        } else
        {
            animator.SetTrigger("Reload");
            currentClip = currentAmmo;
            currentAmmo = 0;
        }
        // Debug.Log("Reloading!");
        // Debug.Log("Current ammo: " + currentAmmo);
    }
}