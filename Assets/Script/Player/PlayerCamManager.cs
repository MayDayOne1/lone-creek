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

    public void ActivateNormal()
    {
        normalCam.gameObject.SetActive(true);
        aimCam.gameObject.SetActive(false);
        crouchCam.gameObject.SetActive(false);
        crouchAimCam.gameObject.SetActive(false);
    }
    public void ActivateAim()
    {
        normalCam.gameObject.SetActive(false);
        aimCam.gameObject.SetActive(true);
        crouchCam.gameObject.SetActive(false);
        crouchAimCam.gameObject.SetActive(false);
    }
    public void ActivateCrouch()
    {
        normalCam.gameObject.SetActive(false);
        aimCam.gameObject.SetActive(false);
        crouchCam.gameObject.SetActive(true);
        crouchAimCam.gameObject.SetActive(false);
    }
    public void ActivateCrouchAim()
    {
        normalCam.gameObject.SetActive(false);
        aimCam.gameObject.SetActive(false);
        crouchCam.gameObject.SetActive(false);
        crouchAimCam.gameObject.SetActive(true);
    }

    public void EnableAll(bool enable)
    {
        normalCam.enabled = enable;
        aimCam.enabled = enable;
        crouchCam.enabled = enable;
        crouchAimCam.enabled = enable;
    }
}