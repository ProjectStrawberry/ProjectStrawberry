using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightStateMachine : StateMachine
{
    public CrystalKnight CrystalKnight { get; private set; }
    
    public CrystalKnightIdleState IdleState { get; private set; }
    public CrystalKnightAttackState AttackState { get; private set; }

    public CrystalKnightStateMachine(CrystalKnight crystalKnight)
    {
        this.CrystalKnight = crystalKnight;

        IdleState = new CrystalKnightIdleState(this);
        AttackState = new CrystalKnightAttackState(this);
    }
}
