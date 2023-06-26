using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    private Animator anim;
    private string currentState;

    private const int CROUCHING_LAYER = 1;
    private const int THROWING_LAYER = 2;
    private const int PISTOL_LAYER = 3;
    private const int PISTOL_CROUCH_LAYER = 4;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetCrouch(bool isCrouching)
    {
        if (isCrouching)
            anim.SetLayerWeight(CROUCHING_LAYER, 1);
        else
            anim.SetLayerWeight(CROUCHING_LAYER, 0);
    }

    public void SetThrow(bool isGonnaThrow)
    {
        if (isGonnaThrow)
            anim.SetLayerWeight(THROWING_LAYER, 1);
        else
            anim.SetLayerWeight(THROWING_LAYER, 0);
    }

    public void SetPistol(bool hasPistol)
    {
        if (hasPistol)
            anim.SetLayerWeight(PISTOL_LAYER, 1);
        else
            anim.SetLayerWeight(PISTOL_LAYER, 0);
    }

    public void SetPistolCrouch(bool hasPistolWhenCrouching)
    {
        if (hasPistolWhenCrouching)
            anim.SetLayerWeight(PISTOL_CROUCH_LAYER, 1);
        else
            anim.SetLayerWeight(PISTOL_CROUCH_LAYER, 0);
    }

    public void DisableAllLayers()
    {
        anim.SetLayerWeight(CROUCHING_LAYER, 0);
        anim.SetLayerWeight(THROWING_LAYER, 0);
        anim.SetLayerWeight(PISTOL_LAYER, 0);
        anim.SetLayerWeight(PISTOL_CROUCH_LAYER, 0);
    }

    public void SetBool(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void SetFloat(string name, float value)
    {
        anim.SetFloat(name, value);
    }
}
