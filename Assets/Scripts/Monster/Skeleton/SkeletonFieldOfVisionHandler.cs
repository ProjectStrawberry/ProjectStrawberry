using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFieldOfVisionHandler : MonoBehaviour
{
    public Skeleton Skeleton;

    private void Awake()
    {
        Skeleton = GetComponentInParent<Skeleton>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !Skeleton.stateMachine.IsCurrentStateAttackState())
        {
            Skeleton.targetPlayer = other.GetComponent<Player>();
            Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.ChasingState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !Skeleton.stateMachine.IsCurrentStateAttackState())
        {
            Skeleton.targetPlayer = null;
            Skeleton.stateMachine.ChangeState(Skeleton.stateMachine.ChasingState);
        }
    }
}
