using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonWalkState : SkeletonBaseState
{
    private bool isRight = true;
    private float walkTimer = 0f;
    private float changeToIdleTime = 3f;
    
    public SkeletonWalkState(SkeletonStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        walkTimer = 0f;
        stateMachine.Skeleton.OnCollide += OnCollision;

        // 처음 걷는 방향을 랜덤화
        FirstFlip();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Skeleton.AnimationData.WalkParameterHash);
        walkTimer = 0f;
        stateMachine.Skeleton.OnCollide -= OnCollision;
    }
    
    public override void Update()
    {
        base.Update();

        // Skeleton 이동
        stateMachine.Skeleton.rigidbody.velocity = new Vector2((isRight ? 1 : -1) * 2, stateMachine.Skeleton.rigidbody.velocity.y);
        
        // 발판이 있는지 없는지 체크
        RaycastHit2D hit = Physics2D.Raycast(stateMachine.Skeleton.groundCheck.position, Vector3.down,
            stateMachine.Skeleton.groundCheckDistance, stateMachine.Skeleton.groundLayer);
        if (hit.collider == null)
        {
            Flip();
        }

        walkTimer += Time.deltaTime;
        if (walkTimer >= changeToIdleTime)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }

    private void Flip()
    {
        isRight = !isRight;
        stateMachine.Skeleton.transform.localScale = new Vector3(isRight ? 2 : -2, 2, 1);
    }

    private void FirstFlip()
    {
        int rand = Random.Range(0, 10);
        if (rand % 2 == 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        stateMachine.Skeleton.transform.localScale = new Vector3(isRight ? 2 : -2, 2, 1);
    }

    private void OnCollision(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Flip();
        }
    }
}
