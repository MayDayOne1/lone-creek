using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChooseWeapon;

public class PlayerAim : MonoBehaviour
{
    // draw line
    // change / move camera
    // change player speed
    // change player rotation speed
    // add and change animation

    // Start is called before the first frame update
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform ReleasePosition;
    [SerializeField] private float ThrowStrength;
    [SerializeField][Range(10, 100)] private int LinePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;

    public CinemachineFreeLook cam;
    public ChooseWeapon chooseWeapon;
    public void Aim()
    {
        if (chooseWeapon.weaponSelected == WEAPONS.THROWABLE)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints + 1);
            Vector3 startPos = ReleasePosition.position;
            Vector3 startVelocity = ThrowStrength * cam.transform.forward;
            int i = 0;
            lineRenderer.SetPosition(i, startPos);
            for(float time = 0; time < TimeBetweenPoints; time += TimeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;
                point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                lineRenderer.SetPosition(i, point);
            }
        }
    }
}
