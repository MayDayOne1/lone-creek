using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamManager : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook NormalCam;
    [SerializeField] private CinemachineFreeLook AimCam;
    [SerializeField] private CinemachineFreeLook CrouchCam;
    [SerializeField] private CinemachineFreeLook CrouchAimCam;

    public void ActivateNormal()
    {
        NormalCam.gameObject.SetActive(true);
        AimCam.gameObject.SetActive(false);
        CrouchCam.gameObject.SetActive(false);
        CrouchAimCam.gameObject.SetActive(false);
    }
    public void ActivateAim()
    {
        NormalCam.gameObject.SetActive(false);
        AimCam.gameObject.SetActive(true);
        CrouchCam.gameObject.SetActive(false);
        CrouchAimCam.gameObject.SetActive(false);
    }
    public void ActivateCrouch()
    {
        NormalCam.gameObject.SetActive(false);
        AimCam.gameObject.SetActive(false);
        CrouchCam.gameObject.SetActive(true);
        CrouchAimCam.gameObject.SetActive(false);
    }

    public void ActivateCrouchAim()
    {
        NormalCam.gameObject.SetActive(false);
        AimCam.gameObject.SetActive(false);
        CrouchCam.gameObject.SetActive(false);
        CrouchAimCam.gameObject.SetActive(true);
    }

    public void EnableAll(bool enable)
    {
        NormalCam.enabled = enable;
        AimCam.enabled = enable;
        CrouchCam.enabled = enable;
        CrouchAimCam.enabled = enable;
    }
}