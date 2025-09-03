using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullStateMachine : StateMachine
{
    public FloatingSkull FloatingSkull { get; private set; }
    
    public FloatingSkullIdleState IdleState { get; private set; }
    public FloatingSkullAttackState AttackState { get; private set; }

    public FloatingSkullStateMachine(FloatingSkull floatingSkull)
    {
        this.FloatingSkull = floatingSkull;

        IdleState = new FloatingSkullIdleState(this);
        AttackState = new FloatingSkullAttackState(this);
    }
}
