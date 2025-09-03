using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullAnimationHandler : MonoBehaviour
{
    public FloatingSkull FloatingSkull;

    private void Awake()
    {
        FloatingSkull = GetComponentInParent<FloatingSkull>();
    }

    public void StartHurtAnimation()
    {
        FloatingSkull.Animator.SetBool(FloatingSkull.AnimationData.HurtParameterHash, true);
    }

    public void EndHurtAnimation()
    {
        FloatingSkull.Animator.SetBool(FloatingSkull.AnimationData.HurtParameterHash, false);
    }
}
