using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Zenject;

public class PlayerCamManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private CinemachineVirtualCamera normalCam;
    [SerializeField] private CinemachineVirtualCamera aimCam;
    [SerializeField] private CinemachineVirtualCamera crouchCam;
    [SerializeField] private CinemachineVirtualCamera crouchAimCam;
    [SerializeField] private Transform cameraLookAt;

    [SerializeField] private float cameraAngleOverride = 0f;

    [SerializeField] private float impulseShootForce = .5f;
    [SerializeField] private float impulseDamageForce = -.1f;

    [Inject] private CinemachineImpulseSource impulseSource;

    private Vector2 lookVector;

    private float topClamp = 70f;
    private float bottomClamp = -30f;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private void Start()
    {
        EnableAll(true);
        ActivateNormal();

        settingsMenu.LoadSensitivity();
    }

    private void OnEnable()
    {
        lookInput.action.Enable();
    }
    private void OnDisable()
    {
        lookInput.action.Disable();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

        lookVector = lookInput.action.ReadValue<Vector2>();

        cinemachineTargetYaw += lookVector.x * PlayerParams.sensitivityX * deltaTimeMultiplier;
        cinemachineTargetPitch += lookVector.y * PlayerParams.sensitivityY * deltaTimeMultiplier;

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        cameraLookAt.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
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
    private void SetTopPriority(CinemachineVirtualCamera cam)
    {
        cam.Priority = 100;
    }
    private void SetLowestPriority(CinemachineVirtualCamera cam)
    {
        cam.Priority = 1;
    }

    public void ShootCamShake()
    {
        if(IsCamShakeEnabled)
        {
            impulseSource.GenerateImpulseWithForce(impulseShootForce);
        }
    }

    public void DamageCamShake()
    {
        if(IsCamShakeEnabled)
        {
            impulseSource.GenerateImpulseWithForce(impulseDamageForce);
        }
    }

    private bool IsCamShakeEnabled => PlayerPrefs.GetInt("isCamShakeEnabled") == 1;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return input.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}