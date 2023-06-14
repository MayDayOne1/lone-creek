using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using TMPro;
using static ChooseWeapon;
using UnityEngine.UI;

public class PlayerShootingManager : MonoBehaviour
{
    [Header("GENERAL")]
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private Rig aimRig;
    private ChooseWeapon chooseWeapon;
    private Animator animator;
    private PlayerInteract playerInteract;
    private PlayerController playerController;
    private float aimRigWeight;
    private Vector3 mouseWorldPos = Vector3.zero;
    public Camera cam;
    public CinemachineFreeLook normalCam;
    public CinemachineFreeLook AimCam;
    public Image Crosshair;


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
    [SerializeField] private TextMeshProUGUI ClipUI;
    public TextMeshProUGUI TotalAmmoUI;
    public int maxAmmo = 24;
    public int currentAmmo = 0;
    private int clipCapacity = 8;
    private int currentClip;
    private float cooldown = .5f;
    private float cooldownTimer;
    private Transform hitTransform = null;
    public bool IsAimingPistol = false;
    public GameObject hitEffect;
    public ParticleSystem particles;
    public float PistolDamage = .2f;
    public bool CanShoot = false;

    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animator = GetComponent<Animator>();
        playerInteract = GetComponent<PlayerInteract>();
        playerController = GetComponent<PlayerController>();

        AimCam.gameObject.SetActive(false);
        currentClip = 0;
        cooldownTimer = cooldown;

        ClipUI.text = currentClip.ToString();
        TotalAmmoUI.text = currentAmmo.ToString();
        Crosshair.gameObject.SetActive(false);

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

    public void SetAmmo(int ammo)
    {
        currentClip = clipCapacity;
        currentAmmo = ammo - currentClip;
        ClipUI.text = currentClip.ToString();
        TotalAmmoUI.text = currentAmmo.ToString();
    }
    private void EnableAim()
    {
        Crosshair.gameObject.SetActive(true);
        AimCam.gameObject.SetActive(true);
        normalCam.gameObject.SetActive(false);
        aimRigWeight = 1f;
    }
    private void DisableAim()
    {
        Crosshair.gameObject.SetActive(false);
        AimCam.gameObject.SetActive(false);
        normalCam.gameObject.SetActive(true);
        aimRigWeight = 0f;
        playerController.speed = playerController.runSpeed;

    }
    private void AimTowardsCrosshair()
    {
        
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
        ClipUI.text = currentClip.ToString();
        if (hitTransform != null)
        {
            GameObject hitParticles = Instantiate(hitEffect, mouseWorldPos, Quaternion.identity);
            Destroy(hitParticles, 2.0f);
            if(hitTransform.tag == "Enemy")
            {
                hitTransform.gameObject.GetComponentInParent<AI>().TakeDamage(PistolDamage);
            }
        }
    }
    private void DisablePistol()
    {
        chooseWeapon.SelectNone();
        DisableAim();
    }
    private void ShootPistol()
    {
        if (currentClip < 1)
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
        else
        {
            DisablePistol();
        }
        
        
    }
    public void Aim()
    {
        AimTowardsCrosshair();
        aimRig.weight = Mathf.Lerp(aimRigWeight, aimRigWeight, Time.deltaTime * 20f);
        float aimValue = aimAction.action.ReadValue<float>();
        // Debug.Log("Aim value " + aimValue);
        if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
        {
            if(aimValue == 1f)
            {
                AimCam.gameObject.SetActive(true);
                StartAimingThrowable();
            } else
            {
                AimCam.gameObject.SetActive(false);
                StopAimingThrowable();
            }
        } else if (chooseWeapon.weaponSelected == WEAPONS.PRIMARY)
        {
            if(aimValue == 1f)
            {
                IsAimingPistol = true;
                EnableAim();
            } else
            {
                IsAimingPistol = false;
                DisableAim();
            }
        } else if (chooseWeapon.weaponSelected == WEAPONS.NONE)
        {
            DisableAim();
            IsAimingPistol = false;
        }   
    }
    public void Shoot()
    {
        if(IsAimingThrowable && playerInteract.Throwable.activeSelf)
        {
            Throw();
        } else if (IsAimingPistol && playerInteract.Pistol.activeSelf && CanShoot)
        {
            ShootPistol();
        }
    }
    public void Reload()
    {
        if(currentAmmo < 1 && currentClip < 1)
        {
            DisablePistol();
            return;
        } else if (currentClip >= 8 || currentAmmo < 1)
        {
            return;
        } else if(currentClip < 8)
        {
            animator.SetTrigger("Reload");
            int ammoDiff = clipCapacity - currentClip;
            int ammoToReload = currentAmmo < ammoDiff ? ammoToReload = currentAmmo : ammoToReload = ammoDiff;
            currentClip += ammoToReload;
            currentAmmo -= ammoToReload;
            ClipUI.text = currentClip.ToString();
            TotalAmmoUI.text = currentAmmo.ToString();
        }
        // Debug.Log("Reloading!");
        // Debug.Log("Current ammo: " + currentAmmo);
    }
}