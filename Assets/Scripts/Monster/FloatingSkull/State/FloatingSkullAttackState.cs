using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkullAttackState : FloatingSkullBaseState
{
    private float fireTimer = 0;
    
    public FloatingSkullAttackState(FloatingSkullStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.FloatingSkull.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.FloatingSkull.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // FloatingSkull이 Attack 상태일 때는 플레이어를 계속 쳐다봐야 한다.
        FacingToPlayer();
        
        fireTimer += Time.deltaTime;
        if (fireTimer >= stateMachine.FloatingSkull.StatData.projectileFireRate)
        {
            fireTimer = 0;
            // 투사체 발사 메서드
            FireProjectile();
            // 아직 시야 안에 플레이어가 있는지 확인하는 메서드
            if (!IsPlayerInFieldOfVision())
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    private void FacingToPlayer()
    {
        // 게임 매니저 같은 곳에서 플레이어를 계속 추격해야 할듯
        var dir = Mathf.Sign(PlayerManager.Instance.player.gameObject.transform.position.x
                             - stateMachine.FloatingSkull.transform.position.x);
        
        stateMachine.FloatingSkull.transform.localScale = new Vector3(dir, 1, 1);
    }
    
    private void FireProjectile()
    {
        Debug.Log("투사체 발사" + stateMachine.FloatingSkull.name);
        // 게임 매니저 같은 곳에서 플레이어를 계속 추격해야 할듯
        stateMachine.FloatingSkull.ProjectileHandler.Attack(PlayerManager.Instance.player.gameObject);
    }

    private bool IsPlayerInFieldOfVision()
    {
        if (stateMachine.FloatingSkull.targetPlayer == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
