using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook normalCam;
    [SerializeField] private CinemachineFreeLook aimCam;
    [SerializeField] private CinemachineFreeLook crouchCam;
    [SerializeField] private CinemachineFreeLook crouchAimCam;

    private CinemachineFreeLook activeCam;

    private void Start()
    {
        EnableAll(true);
        ActivateNormal();
    }

    public void ActivateNormal()
    {
        SetTopPriority(normalCam);
        SetLowestPriority(aimCam);
        SetLowestPriority(crouchCam);
        SetLowestPriority(crouchAimCam);
    }
    public void ActivateAim()
    {
        SetTopPriority(aimCam);
        SetLowestPriority(normalCam);
        SetLowestPriority(crouchCam);
        SetLowestPriority(crouchAimCam);
    }
    public void ActivateCrouch()
    {
        SetTopPriority(crouchCam);
        SetLowestPriority(normalCam);
        SetLowestPriority(aimCam);
        SetLowestPriority(crouchAimCam);
    }
    public void ActivateCrouchAim()
    {
        SetTopPriority(crouchAimCam);
        SetLowestPriority(normalCam);
        SetLowestPriority(aimCam);
        SetLowestPriority(crouchCam);
    }

    public void EnableAll(bool enable)
    {
        normalCam.gameObject.SetActive(enable);
        aimCam.gameObject.SetActive(enable);
        crouchCam.gameObject.SetActive(enable);
        crouchAimCam.gameObject.SetActive(enable);

        normalCam.enabled = enable;
        aimCam.enabled = enable;
        crouchCam.enabled = enable;
        crouchAimCam.enabled = enable;
    }
    private void SetTopPriority(CinemachineFreeLook cam)
    {
        cam.Priority = 100;
        activeCam = cam;
    }
    private void SetLowestPriority(CinemachineFreeLook cam)
    {
        cam.Priority = 1;
    }
}