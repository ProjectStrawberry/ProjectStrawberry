using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChasingState : SkeletonBaseState
{
    private float chasingTimer = 0f;
    private bool isRight = true;
    private bool canMove = true;
    
    public SkeletonChasingState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Chasing 상태 돌입");
        StartAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        stateMachine.Skeleton.OnCollide += OnCollision;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        stateMachine.Skeleton.OnCollide -= OnCollision;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Skeleton.targetPlayer == null)
            return;

        if (canMove)
        {
            WalkToPlayer();
        }
        else
        {
            WaitPlayer();
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
        
        stateMachine.Skeleton.rigidbody.velocity = new Vector2(dir * stateMachine.Skeleton.StatData.walkSpeed,
            stateMachine.Skeleton.rigidbody.velocity.y);
    }

    private void WaitPlayer()
    {
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        StartAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
        
        CheckCanMove();
    }

    private void CheckCanMove()
    {
        var skeletonPos = stateMachine.Skeleton.transform.position;
        var targetPos = new Vector2(stateMachine.Skeleton.targetPlayer.transform.position.x, skeletonPos.y);

        var dir = Mathf.Sign(targetPos.x - skeletonPos.x);

        if (dir * stateMachine.Skeleton.transform.localScale.x < 0f)
        {
            canMove = true;
            StopAnimation(stateMachine.Skeleton.AnimationData.IdleParameterHash);
            StartAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        }
    }
    
    private void OnCollision(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            canMove = false;
        }
    }
}
