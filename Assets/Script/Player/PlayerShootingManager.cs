using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using TMPro;
using UnityEngine.UI;

public class PlayerShootingManager : MonoBehaviour
{
    [Header("GENERAL")]
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private Rig aimRig;
    private ChooseWeapon chooseWeapon;
    private PlayerAnimManager animManager;
    private PlayerInteract playerInteract;
    private PlayerController playerController;
    private PlayerCamManager camManager;
    private PlayerAmmoManager ammoManager;
    private float aimRigWeight;
    private Vector3 mouseWorldPos = Vector3.zero;
    public Camera cam;


    [Header("THROW")]
    [SerializeField] private Transform PlayerBottle;
    [SerializeField] private GameObject ThrowablePlayerBottle;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float ThrowStrength = 20f;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    [SerializeField] private AudioClip audioClip;
    private GameObject BottleToInstantiate;
    public bool IsAimingThrowable = false;

    private const string IS_AIMING_THROWABLE = "isAimingThrowable";
    private const string THROW = "Throw";

    [Header("PISTOL")]
    [SerializeField] private LayerMask aimColliderLayerMask = new();
    [SerializeField] private Transform dummyTransform;
    [SerializeField] private Image Crosshair;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private ParticleSystem fireEffect;
    private readonly float cooldown = .5f;
    private float cooldownTimer;
    private Transform hitTransform = null;
    public bool IsAimingPistol = false;
    public float PistolDamage = .2f;
    public bool CanShoot = false;

    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animManager = GetComponent<PlayerAnimManager>();
        playerInteract = GetComponent<PlayerInteract>();
        playerController = GetComponent<PlayerController>();
        camManager = GetComponent<PlayerCamManager>();
        ammoManager = GetComponent<PlayerAmmoManager>();

        cooldownTimer = cooldown;
        Crosshair.gameObject.SetActive(false);
    }
    private void StartAimingPistol()
    {
        IsAimingPistol = true;
        Crosshair.gameObject.SetActive(true);
        if (playerController.IsCrouching)
        {
            camManager.ActivateCrouchAim();
        } else
        {
            camManager.ActivateAim();
        }
        aimRigWeight = 1f;
    }
    private void StopAimingPistol()
    {
        IsAimingPistol = false;
        Crosshair.gameObject.SetActive(false);
        if (playerController.IsCrouching)
        {
            camManager.ActivateCrouch();
        }
        else
        {
            camManager.ActivateNormal();
        }
        aimRigWeight = 0f;
        playerController.SetSpeed(playerController.runSpeed);

    }
    private void AimTowardsCrosshair()
    {
        
        Vector2 screenCenterPoint = new (Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            dummyTransform.position = hit.point;
            mouseWorldPos = hit.point;
            hitTransform = hit.transform;

            if (hitTransform.CompareTag("Enemy")) Crosshair.color = Color.red;
            else Crosshair.color = Color.white;
        }
    }
    private void StartAimingThrowable()
    {
        if(playerInteract.hasThrowable)
        {
            IsAimingThrowable = true;
            animManager.SetThrow(true);
            animManager.SetBool(IS_AIMING_THROWABLE, true);
            DrawLine();

            if (playerController.IsCrouching) camManager.ActivateCrouchAim();
            else camManager.ActivateAim();
        }
        
    }
    private void StopAimingThrowable()
    {
        IsAimingThrowable = false;
        animManager.SetThrow(false);
        lineRenderer.enabled = false;
        animManager.SetBool(IS_AIMING_THROWABLE, false);

        if (playerController.IsCrouching) camManager.ActivateCrouch();
        else camManager.ActivateNormal();
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
        animManager.SetTrigger(THROW);
        playerInteract.Throwable.SetActive(false);
        PlayerBottle.gameObject.SetActive(false);
        Transform instancePos = PlayerBottle.transform;
        BottleToInstantiate = Instantiate(ThrowablePlayerBottle, instancePos.position, instancePos.rotation);
        Rigidbody bottleRb = BottleToInstantiate.GetComponent<Rigidbody>();
        bottleRb.AddForce(cam.transform.forward * ThrowStrength, ForceMode.VelocityChange);
        if(BottleToInstantiate != null) Destroy(BottleToInstantiate, 2f);
        playerInteract.hasThrowable = false;
        IsAimingThrowable = false;
        chooseWeapon.ThrowableBG.SetActive(false);
    }
    private void Fire()
    {
        fireEffect.Play();
        playerInteract.audioSource.Play();
        ammoManager.DecrementClip();
        if (hitTransform != null)
        {
            GameObject hitParticles = Instantiate(hitEffect, mouseWorldPos, Quaternion.identity);
            Destroy(hitParticles, .1f);
            if(hitTransform.CompareTag("Enemy"))
            {
                hitTransform.gameObject.GetComponentInParent<AI>().TakeDamage(PistolDamage);
            }
        }
    }
    public void DisablePistol()
    {
        chooseWeapon.SelectNone();
        StopAimingPistol();
    }
    private void CheckIfCanShoot()
    {
        if (!ammoManager.HasAmmoToShoot())
        {
            ammoManager.Reload();
        }
        if (Time.time - cooldownTimer < cooldown)
        {
            return;
        }
        else if (ammoManager.HasAmmoToShoot())
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
        if (chooseWeapon.IsThrowableSelected)
        {
            StopAimingPistol();
            if (aimValue == 1f) StartAimingThrowable();
            else StopAimingThrowable();
        } else if (chooseWeapon.IsPrimarySelected)
        {
            if(aimValue == 1f) StartAimingPistol();
            else StopAimingPistol();
        } else
        {
            StopAimingThrowable();
            StopAimingPistol();
        }   
    }
    public void Shoot()
    {
        if(IsAimingThrowable && playerInteract.Throwable.activeSelf)
        {
            Throw();
        } else if (IsAimingPistol && playerInteract.Pistol.activeSelf && CanShoot)
        {
            CheckIfCanShoot();
        }
    }
}