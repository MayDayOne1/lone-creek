using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ChooseWeapon;

public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform ReleasePosition;
    [SerializeField] private float ThrowStrength;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;

    private PlayerController playerController;
    private ChooseWeapon chooseWeapon;
    private Animator animator;
    private PlayerInteract playerInteract;
    public Camera cam;
    public Rigidbody PlayerBottle;

    public bool IsAimingThrowable = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        chooseWeapon = GetComponent<ChooseWeapon>();
        animator = GetComponent<Animator>();
        playerInteract = GetComponent<PlayerInteract>();
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
                DrawLine();
                
            } else
            {
                IsAimingThrowable = false;
                animator.SetLayerWeight(2, 0);
                lineRenderer.enabled = false;
            }
            
        }        
    }

    private void DrawLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints + 1);
        Vector3 startPos = ReleasePosition.position;
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

    public void Shoot()
    {
        if(IsAimingThrowable)
        {
            playerController.CalculateCharacterRotation();
            animator.SetTrigger("Throw");
            PlayerBottle.isKinematic = false;
            PlayerBottle.AddForce(cam.transform.forward * ThrowStrength, ForceMode.VelocityChange);
            chooseWeapon.weaponSelected = WEAPONS.NONE;
            IsAimingThrowable = false;
            animator.SetLayerWeight(2, 0);
            playerInteract.Throwable.SetActive(false);
        }
        
    }
}
