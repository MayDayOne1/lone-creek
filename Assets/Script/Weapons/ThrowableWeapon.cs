using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class ThrowableWeapon : MonoBehaviour, IWeapon
{
    public GameObject throwable;
    public GameObject player;
    public CanvasGroup throwableBG;
    public bool isSelected = false;
    public bool isAiming = false;

    [SerializeField] private GameObject throwablePlayerBottle;
    [SerializeField] private AudioClip onSelect;

    [Header("LINE RENDERING")]
    [SerializeField] private float throwStrength = 20f;
    [SerializeField][Range(10, 100)] private int linePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

    
    private bool isPlayingSound = false;

    private LineRenderer lineRenderer;
    private GameObject bottleInstance;
    private AudioSource audioSource;

    [Inject] PlayerController controller;
    [Inject] PlayerCamManager camManager;
    [Inject] PlayerAnimManager animManager;
    [Inject] PlayerShootingManager shootingManager;
    [Inject] Camera cam;

    private const string IS_AIMING_THROWABLE = "isAimingThrowable";
    private const string THROW = "Throw";

    

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int playerBottleThrowCount = 0;
#endif

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = onSelect;

        Disable();
        throwableBG.interactable = false;
        throwableBG.blocksRaycasts = false;
        lineRenderer = throwable.GetComponent<LineRenderer>();
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
        throwable.SetActive(false);

        animManager.SetThrow(false);
        if (lineRenderer != null) lineRenderer.enabled = false;

        isSelected = false;
        CamSetup();
        EnableUI(false);
    }
    

    public void EnableUI(bool enable)
    {
        if(enable)
        {
            shootingManager.previousWeapon.EnableUI(false);
            throwableBG.DOFade(1f, 0f);
        }
        else
        {
            throwableBG.DOFade(0f, 0f);
        }
    }

    public void PlaySelectionSound()
    {
        Timing.RunCoroutine(PlaySound());
    }

    IEnumerator<float> PlaySound()
    {
        if(!isSelected && !isPlayingSound)
        {
            audioSource.PlayOneShot(onSelect);
            isPlayingSound = true;
            yield return Timing.WaitForSeconds(onSelect.length);
        }
        isPlayingSound = false;
    }

    public void Select()
    {
        if(PlayerInteract.hasThrowable)
        {
            shootingManager.previousWeapon = shootingManager.currentWeapon;
            throwable.SetActive(true);

            PlaySelectionSound();

            isSelected = true;
            EnableUI(true);
        }
    }

    public void Shoot()
    {
        if(isAiming)
        {
            animManager.SetTrigger(THROW);

            InstanceBottle();

            Disable();
            PlayerInteract.hasThrowable = false;
            isAiming = false;
            StopAim();

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            playerBottleThrowCount++;
#endif
        }
    }

    private void InstanceBottle()
    {
        Transform instancePos = throwable.transform;
        bottleInstance = Instantiate(throwablePlayerBottle, instancePos.position, instancePos.rotation);
        Rigidbody bottleRb = bottleInstance.GetComponent<Rigidbody>();
        bottleRb.AddForce(cam.transform.forward * throwStrength, ForceMode.Impulse);
        if (bottleInstance != null) Destroy(bottleInstance, 2f);
    }

    private IEnumerator<float> DrawLine()
    {
        while (isAiming)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints + 1);
            Vector3 startPos = throwable.transform.position;
            Vector3 startVelocity = throwStrength * cam.transform.forward;
            int i = 0;
            lineRenderer.SetPosition(i, startPos);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;
                point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                lineRenderer.SetPosition(i, point);
            }
            yield return Timing.WaitForOneFrame;
        }
        lineRenderer.enabled = false;
    }

    public void StartAim()
    {
        if(isSelected)
        {
            isAiming = true;
            animManager.SetThrow(true);
            animManager.SetBool(IS_AIMING_THROWABLE, true);

            AimCamSetup();

            Timing.RunCoroutine(DrawLine());
        }
    }

    public void StopAim()
    {
        isAiming = false;
        animManager.SetThrow(false);
        animManager.SetBool(IS_AIMING_THROWABLE, false);

        CamSetup();

        StopCoroutine(DrawLine());
    }
}
