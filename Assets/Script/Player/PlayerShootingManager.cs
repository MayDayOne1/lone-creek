using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using System.Collections;

public class PlayerShootingManager : MonoBehaviour
{
    public Camera cam;

    [Header("THROW")]
    public GameObject playerBottle;
    public bool IsAimingThrowable = false;
    [SerializeField] private GameObject ThrowablePlayerBottle;
    [SerializeField] private float ThrowStrength = 20f;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    private LineRenderer lineRenderer;
    private GameObject bottleInstance;

    [Header("PISTOL")]
    public GameObject pistol;
    public bool isAimingPistol = false;
    public bool CanShoot = false;
    public float pistolDamage = .2f;
    [SerializeField] private Rig aimRig;
    [SerializeField] private LayerMask aimColliderLayerMask = new();
    [SerializeField] private Transform dummyTransform;
    [SerializeField] private Image crosshair;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private AudioSource pistolAudioSource;
    private Vector3 mouseWorldPos = Vector3.zero;
    private readonly float cooldown = .5f;
    private float cooldownTimer;
    private readonly float aimRigWeight;
    private Transform hitTransform = null;

    private const string IS_AIMING_THROWABLE = "isAimingThrowable";
    private const string THROW = "Throw";
    private const string IS_AIMING_PISTOL = "isAimingPistol";

    private ChooseWeapon chooseWeapon;
    private PlayerAnimManager animManager;
    private PlayerController controller;
    private PlayerCamManager camManager;
    private PlayerAmmoManager ammoManager;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int playerBottleThrowCount = 0;
    public static int playerShotsFiredCount = 0;
    public static int playerShotsHit = 0;
    public static int playerTimesAimed = 0;
    public static float playerTimeSpentAiming = 0f;
#endif

    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animManager = GetComponent<PlayerAnimManager>();
        controller = GetComponent<PlayerController>();
        camManager = GetComponent<PlayerCamManager>();
        ammoManager = GetComponent<PlayerAmmoManager>();

        lineRenderer = playerBottle.GetComponent<LineRenderer>();

        cooldownTimer = cooldown;
        aimRig.weight = 0f;
        crosshair.gameObject.SetActive(false);
    }

    public void SetupSelectNone()
    {
        SetAimRigWeight(0f);
        SetCrosshairVisibility(false);
        StopAimingThrowable();
        StopAimingPistol();
        playerBottle.SetActive(false);
        pistol.SetActive(false);
    }
    private IEnumerator CountAimingTime()
    {
        while(IsAimingThrowable || isAimingPistol)
        {
            playerTimeSpentAiming += Time.deltaTime;
            yield return null;
        }
    }
    public void SetAimRigWeight(float newWeight)
    {
        LeanTween.value(gameObject, aimRigWeight, newWeight, .15f)
            .setOnUpdate((value) =>
            {
                aimRig.weight = value;
            });
    }
    public void SetCrosshairVisibility(bool isVisible) => crosshair.gameObject.SetActive(isVisible);
    private void StartAimingPistol()
    {
        SetAimRigWeight(1f);
        isAimingPistol = true;
        SetCrosshairVisibility(true);

        animManager.SetBool(IS_AIMING_PISTOL, true);
        if (controller.IsCrouching)
        {
            camManager.ActivateCrouchAim();
        }
        else
        {
            camManager.ActivateAim();
        }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        StartCoroutine(CountAimingTime());
#endif
    }
    public void StopAimingPistol()
    {
        SetAimRigWeight(0f);
        isAimingPistol = false;
        SetCrosshairVisibility(false);

        animManager.SetBool(IS_AIMING_PISTOL, false);
        if (controller.IsCrouching)
        {
            camManager.ActivateCrouch();
        }
        else
        {
            camManager.ActivateNormal();
        }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        StopCoroutine(CountAimingTime());
#endif

    }
    private IEnumerator AimTowardscrosshair()
    {
        while(isAimingPistol)
        {
            Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                dummyTransform.position = hit.point;
                mouseWorldPos = hit.point;
                hitTransform = hit.transform;

                if (hitTransform.CompareTag("Enemy")) crosshair.color = Color.red;
                else crosshair.color = Color.white;
            }
            yield return null;
        }
    }
    public void StartAimingThrowable()
    {
        if(PlayerInteract.hasThrowable)
        {
            SetAimRigWeight(0f);
            IsAimingThrowable = true;
            animManager.SetThrow(true);
            animManager.SetBool(IS_AIMING_THROWABLE, true);

            if (controller.IsCrouching)
            {
                camManager.ActivateCrouchAim();
            }
            else
            {
                camManager.ActivateAim();
            }

            StartCoroutine(DrawLine());
        }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        StartCoroutine(CountAimingTime());
#endif
    }
    public void StopAimingThrowable()
    {
        IsAimingThrowable = false;
        animManager.SetThrow(false);
        animManager.SetBool(IS_AIMING_THROWABLE, false);
        lineRenderer.enabled = false;
        if (controller.IsCrouching)
        {
            camManager.ActivateCrouch();
        }
        else
        {
            camManager.ActivateNormal();
        }

        StopCoroutine(DrawLine());

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        StopCoroutine(CountAimingTime());
#endif
    }
    private IEnumerator DrawLine()
    {
        while(IsAimingThrowable)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints + 1);
            Vector3 startPos = playerBottle.transform.position;
            Vector3 startVelocity = ThrowStrength * cam.transform.forward;
            int i = 0;
            lineRenderer.SetPosition(i, startPos);
            for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;
                point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                lineRenderer.SetPosition(i, point);
            }
            yield return null;
        }
    }
    private void Throw()
    {
        animManager.SetTrigger(THROW);
        playerBottle.SetActive(false);

        Transform instancePos = playerBottle.transform;
        bottleInstance = Instantiate(ThrowablePlayerBottle, instancePos.position, instancePos.rotation);
        Rigidbody bottleRb = bottleInstance.GetComponent<Rigidbody>();
        bottleRb.AddForce(cam.transform.forward * ThrowStrength, ForceMode.Impulse);
        if(bottleInstance != null) Destroy(bottleInstance, 2f);

        PlayerInteract.hasThrowable = false;
        IsAimingThrowable = false;
        chooseWeapon.ThrowableBG.SetActive(false);
        StopAimingThrowable();
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerBottleThrowCount++;
#endif
    }
    private void Fire()
    {
        fireEffect.Play();
        pistolAudioSource.Play();
        ammoManager.DecrementClip();
        if (hitTransform != null)
        {
            GameObject hitParticles = Instantiate(hitEffect, mouseWorldPos, Quaternion.identity);
            Destroy(hitParticles, .1f);
            if(hitTransform.CompareTag("Enemy"))
            {
                hitTransform.GetComponentInParent<AI>().TakeDamage(pistolDamage);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
                playerShotsHit++;
#endif
            }
        }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerShotsFiredCount++;
#endif
    }
    public void DisablePistol()
    {
        chooseWeapon.SelectNone();
        StopAimingPistol();
        animManager.SetPistol(false, controller.IsCrouching);
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
    public void Aim(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (chooseWeapon.IsThrowableSelected)
            {
                StopAimingPistol();
                StartAimingThrowable();
                StartCoroutine(AimTowardscrosshair());
            }
            else if (chooseWeapon.IsPrimarySelected)
            {
                StopAimingThrowable();
                StartAimingPistol();
                StartCoroutine(AimTowardscrosshair());
            }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            playerTimesAimed++;
#endif
        }
        else if(context.canceled)
        {
            StopAimingThrowable();
            StopAimingPistol();
        }
    }
    public void Shoot()
    {
        if(IsAimingThrowable)
        {
            Throw();
        } else if (isAimingPistol && CanShoot)
        {
            CheckIfCanShoot();
        }
    }
}