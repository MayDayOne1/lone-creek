using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ChooseWeapon;

public class PlayerAim : MonoBehaviour
{
    // change / move camera
    // change player speed
    // change player rotation speed
    // add and change animation
    [SerializeField] private InputActionReference aimReference;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform ReleasePosition;
    [SerializeField] private float ThrowStrength;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;

    public Camera Cam;
    public ChooseWeapon chooseWeapon;
    public Animator animator;

    public void Aim(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Performed)
        {
            Debug.Log("performed!");
            if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
            {
                animator.SetLayerWeight(2, 1);
                StartCoroutine(WaitAndDrawLine());
            }
            // lineRenderer.enabled = false;
        } else
        {
            animator.SetLayerWeight(2, 0);
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
        Vector3 startVelocity = ThrowStrength * Cam.transform.forward;
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
}
