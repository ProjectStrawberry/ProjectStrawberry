using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string hurtParameterName = "Hurt";
    [SerializeField] private string dieParameterName = "Die";
    
    public int IdleParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int HurtParameterHash { get; private set; }
    public int DieParameterHash { get; private set; }
    
    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        HurtParameterHash = Animator.StringToHash(hurtParameterName);
        DieParameterHash = Animator.StringToHash(dieParameterName);
    }
}
