using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChasingState : SkeletonBaseState
{
    private float chasingTimer = 0f;
    private float aggroTime = 3f;
    private bool isRight = true;
    
    public SkeletonChasingState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Chasing 상태 돌입");
        chasingTimer = 0;
        StartAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Chasing 상태 퇴장");
        chasingTimer = 0;
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Skeleton.targetPlayer == null)
        {
            WaitPlayer();
        }
        else
        {
            if (CheckCanWalk())
            {
                WalkToPlayer();
            }
            else
            {
                WaitPlayer();
            }
        }
        
        var dist = Mathf.Abs(stateMachine.Skeleton.transform.position.x -
                            PlayerManager.Instance.player.transform.position.x);
        if (dist <= stateMachine.Skeleton.StatData.RushAttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    private void WalkToPlayer()
    {
        var skeletonPos = stateMachine.Skeleton.transform.position;
        var targetPos = new Vector2(stateMachine.Skeleton.targetPlayer.transform.position.x, skeletonPos.y);

        float dir = Mathf.Sign(targetPos.x - skeletonPos.x);
        
        if (dir > 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        stateMachine.Skeleton.transform.localScale = new Vector3(isRight ? 2 : -2, 2, 1);
        
        stateMachine.Skeleton._rigidbody.velocity = new Vector2(dir * stateMachine.Skeleton.StatData.walkSpeed,
            stateMachine.Skeleton._rigidbody.velocity.y);
    }

    private void WaitPlayer()
    {
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        StartAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);

        chasingTimer += Time.deltaTime;
        if (chasingTimer >= aggroTime)
        {
            Debug.Log("Idle 상태로 돌아갑니다");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private bool CheckCanWalk()
    {
        // 발 앞에 장애물이 있는지 없는지 체크
        RaycastHit2D hit = Physics2D.Raycast(stateMachine.Skeleton.groundCheck.position + Vector3.up * 0.5f, 
            Vector3.right * Mathf.Sign(stateMachine.Skeleton.transform.localScale.x)
            , stateMachine.Skeleton.groundCheckDistance, stateMachine.Skeleton.groundLayer);
        if (hit.collider != null)
        {
            return false;
        }
        
        // 발 앞에 발판이 있는지 없는지 체크
        RaycastHit2D hitDown = Physics2D.Raycast(stateMachine.Skeleton.groundCheck.position, 
            Vector3.down, stateMachine.Skeleton.groundCheckDistance, stateMachine.Skeleton.obstacleLayer);
        if (hitDown.collider == null)
        {
            return false;
        }

        return true;
    }
}
