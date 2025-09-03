using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullFieldOfVisionHandler : MonoBehaviour
{
    public FloatingSkull FloatingSkull;
    
    private void Awake()
    {
        FloatingSkull = GetComponentInParent<FloatingSkull>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FloatingSkull.targetPlayer = other.GetComponent<Player>();
            FloatingSkull.stateMachine.ChangeState(FloatingSkull.stateMachine.AttackState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FloatingSkull.targetPlayer = null;
        }
    }
}
