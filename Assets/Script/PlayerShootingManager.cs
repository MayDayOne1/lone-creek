using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ChooseWeapon;

public class PlayerShootingManager : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform ReleasePosition;
    [SerializeField] private float ThrowStrength;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;

    private ChooseWeapon chooseWeapon;
    private Animator animator;
    private PlayerInteract playerInteract;
    public Camera cam;
    public Rigidbody PlayerBottle;

    public bool IsAiming = false;

    private void Start()
    {
        chooseWeapon = GetComponent<ChooseWeapon>();
        animator = GetComponent<Animator>();
        playerInteract = GetComponent<PlayerInteract>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        IsAiming = true;
        
        if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
        {
            animator.SetLayerWeight(2, 1);
            animator.SetTrigger("AimThrowable");
            StartCoroutine(WaitAndDrawLine());
        }
        // lineRenderer.enabled = false;
        if(context.phase == InputActionPhase.Canceled)
        {
            animator.ResetTrigger("AimThrowable");
        }
    }

    public IEnumerator WaitAndDrawLine()
    {
        lineRenderer.enabled = false;
        DrawLine();
        yield return new WaitForSeconds(.05f);
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
        if(IsAiming)
        {
            if (chooseWeapon.weaponSelected == ChooseWeapon.WEAPONS.THROWABLE)
            {
                animator.SetTrigger("Throw");
                PlayerBottle.isKinematic = false;
                PlayerBottle.AddForce(cam.transform.forward * ThrowStrength, ForceMode.VelocityChange);
                // animator.ResetTrigger("Throw");
            }
        }
        
    }
}
