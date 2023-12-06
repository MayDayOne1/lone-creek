using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float layerBlendTime = .15f;

    private PlayerShootingManager shootingManager;

    private const int CROUCHING_LAYER = 1;
    private const int THROWING_LAYER = 2;
    private const int PISTOL_LAYER = 3;
    private const int PISTOL_CROUCH_LAYER = 4;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        shootingManager = GetComponent<PlayerShootingManager>();
    }

    private void SmoothLayerSwitch(int layer, float weight)
    {
        var layerWeight = anim.GetLayerWeight(layer);
        LeanTween.value(gameObject, layerWeight, weight, layerBlendTime)
            .setOnUpdate((value) =>
            {
                anim.SetLayerWeight(layer, value);
            });
    }
    public void SetCrouch(bool isCrouching)
    {
        if (isCrouching)
        {
            SmoothLayerSwitch(CROUCHING_LAYER, 1f);
            if (shootingManager.isPistolEquipped)
            {
                SmoothLayerSwitch(PISTOL_LAYER, 0f);
                SmoothLayerSwitch(PISTOL_CROUCH_LAYER, 1f);
            }
        }
        else
        {
            SmoothLayerSwitch(CROUCHING_LAYER, 0f);
            SmoothLayerSwitch(PISTOL_CROUCH_LAYER, 0f);
            if (shootingManager.isPistolEquipped)
            {
                SmoothLayerSwitch(PISTOL_LAYER, 1f);
            }
            else
            {
                SmoothLayerSwitch(PISTOL_LAYER, 0f);
            }
        }
    }
    public void SetThrow(bool isGonnaThrow)
    {
        if (isGonnaThrow)
        {
            SmoothLayerSwitch(THROWING_LAYER, 1f);
        }
        else
        {
            SmoothLayerSwitch(THROWING_LAYER, 0f);
        }
    }
    public void SetPistol(bool hasPistol, bool isCrouching)
    {
        if (hasPistol)
        {
            if (isCrouching)
            {
                SmoothLayerSwitch(PISTOL_LAYER, 0f);
                SmoothLayerSwitch(PISTOL_CROUCH_LAYER, 1f);
            }
            else
            {
                SmoothLayerSwitch(PISTOL_LAYER, 1f);
                SmoothLayerSwitch(PISTOL_CROUCH_LAYER, 0f);
            }
        }
        else
        {
            SmoothLayerSwitch(PISTOL_LAYER, 0f);
            SmoothLayerSwitch(PISTOL_CROUCH_LAYER, 0f);
            if (isCrouching)
            {
                SmoothLayerSwitch(CROUCHING_LAYER, 1f);
            }
            else
            {
                SmoothLayerSwitch(CROUCHING_LAYER, 0f);
            }
        }
    }
    public void SetBool(string name, bool state)
    {
        anim.SetBool(name, state);
    }
    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }
    public void SetFloat(string name, float value, float dampTime, float time)
    {
        anim.SetFloat(name, value, dampTime, time);
    }
    public void SetAnimator(bool enable) => anim.enabled = enable;
    public void DisableAllLayers()
    {
        anim.SetLayerWeight(CROUCHING_LAYER, 0);
        anim.SetLayerWeight(THROWING_LAYER, 0);
        anim.SetLayerWeight(PISTOL_LAYER, 0);
        anim.SetLayerWeight(PISTOL_CROUCH_LAYER, 0);
    }
}