using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using DG.Tweening;

public class PistolWeapon : MonoBehaviour, IWeapon
{
    public GameObject pistol;
    public GameObject player;
    public CanvasGroup ammoBG;
    public bool isSelected = false;
    public bool isAiming = false;
    public float pistolDamage = .2f;

    PlayerController controller;
    PlayerCamManager camManager;
    PlayerAnimManager animManager;
    PlayerShootingManager shootingManager;
    PlayerAmmoManager ammoManager;

    [SerializeField] private Rig aimRig;
    [SerializeField] private Image crosshair;
    [SerializeField] private LayerMask aimColliderLayerMask = new();
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private AudioSource pistolAudioSource;
    [SerializeField] private Transform aimTarget;

    private Vector3 mouseWorldPos = Vector3.zero;
    private float aimRigWeight;
    private float cooldownTimer;
    private readonly float cooldown = .5f;
    private bool canShoot;
    private Transform hitTransform = null;

    private const string IS_AIMING_PISTOL = "isAimingPistol";

    public bool CanShoot
    {
        get
        {
            if (!ammoManager.HasAmmoToShoot())
            {
                ammoManager.Reload();
            }
            else
            {
                if (Time.time - cooldownTimer >= cooldown)
                {
                    cooldownTimer = Time.time;
                    return true;
                }
            }

            return false;
        }

        set
        {
            canShoot = value;
        }
    }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int playerShotsFiredCount = 0;
    public static int playerShotsHit = 0;
#endif

    void Start()
    {
        controller = player.GetComponent<PlayerController>();
        camManager = player.GetComponent<PlayerCamManager>();
        animManager = player.GetComponent<PlayerAnimManager>();
        shootingManager = player.GetComponent<PlayerShootingManager>();
        ammoManager = player.GetComponent<PlayerAmmoManager>();

        Disable();
        ammoBG.interactable = false;
        ammoBG.blocksRaycasts = false;
        aimRig.weight = 0f;

        cooldownTimer = cooldown;
    }

    public void AimCamSetup()
    {
        if (controller.IsCrouching)
        {
            camManager.ActivateCrouchAim();
        }
        else
        {
            camManager.ActivateAim();
        }
    }

    public void CamSetup()
    {
        if (controller.IsCrouching)
        {
            camManager.ActivateCrouch();
        }
        else
        {
            camManager.ActivateNormal();
        }
    }

    public void Disable()
    {
        shootingManager.previousWeapon = this;
        shootingManager.isPistolEquipped = false;

        pistol.SetActive(false);
        animManager.SetPistol(false, controller.IsCrouching);
        crosshair.gameObject.SetActive(false);
        isSelected = false;

        CamSetup();
        EnableUI(false);
        SetAimRigWeight(0f);
    }

    public void EnableUI(bool enable)
    {
        if(enable)
        {
            shootingManager.previousWeapon.EnableUI(false);
            ammoBG.DOFade(1f, 0f);
        }
        else
        {
            ammoBG.DOFade(0f, 0f);
        }
    }

    public void Select()
    {
        if(PlayerInteract.hasPrimary)
        {
            shootingManager.previousWeapon = shootingManager.currentWeapon;
            shootingManager.isPistolEquipped = true;
            pistol.SetActive(true);
            isSelected = true;
            EnableUI(true);
        }
    }

    public void Shoot()
    {
        if(CanShoot && isAiming)
        {
            Fire();
        }
    }

    public void StartAim()
    {
        if(isSelected)
        {
            isAiming = true;
            SetCrosshair(true);

            SetAimRigWeight(1f);
            animManager.SetPistol(true, controller.IsCrouching);

            animManager.SetBool(IS_AIMING_PISTOL, true);
            AimCamSetup();
            StartCoroutine(AimTowardsCrosshair());
        }
    }

    public void StopAim()
    {
        isAiming = false;
        SetCrosshair(false);

        SetAimRigWeight(0f);
        animManager.SetPistol(false, controller.IsCrouching);

        animManager.SetBool(IS_AIMING_PISTOL, false);
        CamSetup();
        StopCoroutine(AimTowardsCrosshair());
    }

    private IEnumerator AimTowardsCrosshair()
    {
        while (isAiming)
        {
            Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                aimTarget.position = hit.point;
                mouseWorldPos = hit.point;
                hitTransform = hit.transform;

                if (hitTransform.CompareTag("Enemy")) crosshair.color = Color.red;
                else crosshair.color = Color.white;
            }
            yield return null;
        }
    }

    private void SetAimRigWeight(float newWeight)
    {
        LeanTween.value(gameObject, aimRigWeight, newWeight, .15f)
            .setOnUpdate((value) =>
            {
                aimRig.weight = value;
            });
    }
    private void SetCrosshair(bool isVisible) => crosshair.gameObject.SetActive(isVisible);

    private void Fire()
    {
        fireEffect.Play();
        pistolAudioSource.Play();
        ammoManager.DecrementClip();
        if (hitTransform != null)
        {
            GameObject hitParticles = Instantiate(hitEffect, mouseWorldPos, Quaternion.identity);
            Destroy(hitParticles, .1f);
            if (hitTransform.CompareTag("Enemy"))
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
}
