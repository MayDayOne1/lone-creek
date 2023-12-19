using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using DG.Tweening;
using MEC;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class PistolWeapon : MonoBehaviour, IWeapon
{
    public CanvasGroup ammoBG;
    public bool isSelected = false;
    public bool isAiming = false;
    public float pistolDamage = .2f;

    [Inject] PlayerController controller;
    [Inject] PlayerCamManager camManager;
    [Inject] PlayerAnimManager animManager;
    [Inject] PlayerShootingManager shootingManager;
    [Inject] PlayerAmmoManager ammoManager;

    [SerializeField] private Rig aimRig;
    [SerializeField] private Image crosshair;
    [SerializeField] private LayerMask aimColliderLayerMask = new();
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private AudioClip gunshot;
    [SerializeField] private Transform aimTarget;

    [SerializeField] private AudioClip onSelect;

    private Vector3 mouseWorldPos = Vector3.zero;
    private float cooldownTimer;
    private readonly float cooldown = .5f;
    private Transform hitTransform = null;

    private AudioSource audioSource;
    private bool isPlayingSound = false;

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
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = onSelect;

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

        this.gameObject.SetActive(false);
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
    public void PlaySelectionSound()
    {
        Timing.RunCoroutine(PlaySound());
    }

    IEnumerator<float> PlaySound()
    {
        if (!isSelected && !isPlayingSound)
        {
            audioSource.PlayOneShot(onSelect);
            isPlayingSound = true;
            yield return Timing.WaitForSeconds(onSelect.length);
        }
        isPlayingSound = false;
    }

    public void Select()
    {
        if(PlayerParams.hasPrimary)
        {
            shootingManager.previousWeapon = shootingManager.currentWeapon;
            shootingManager.isPistolEquipped = true;
            animManager.SetPistol(true, controller.IsCrouching);
            this.gameObject.SetActive(true);

            PlaySelectionSound();

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
            Timing.RunCoroutine(AimTowardsCrosshair());
        }
    }

    public void StopAim()
    {
        isAiming = false;
        SetCrosshair(false);

        SetAimRigWeight(0f);

        animManager.SetBool(IS_AIMING_PISTOL, false);
        CamSetup();
        StopCoroutine(AimTowardsCrosshair());
    }

    private IEnumerator<float> AimTowardsCrosshair()
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
            yield return Timing.WaitForOneFrame;
        }
    }

    private void SetAimRigWeight(float newWeight)
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, newWeight, .15f);
    }
    private void SetCrosshair(bool isVisible) => crosshair.gameObject.SetActive(isVisible);

    private void Fire()
    {
        fireEffect.Play();
        AudioSource.PlayClipAtPoint(gunshot, transform.position);
        camManager.ShootCamShake();
        ammoManager.DecrementClip();
        if (hitTransform != null)
        {
            GameObject hitParticles = Instantiate(hitEffect, mouseWorldPos, Quaternion.identity);
            Destroy(hitParticles, .1f);
            if (hitTransform.CompareTag("Enemy"))
            {
                hitTransform.GetComponentInParent<AI>().TakeDamage(pistolDamage);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
                PlayerParams.playerShotsHit++;
#endif
            }
        }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerParams.playerShotsFiredCount++;
#endif
    }
}
