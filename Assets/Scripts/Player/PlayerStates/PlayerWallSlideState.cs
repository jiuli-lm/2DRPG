using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space)){
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        if(xInput != 0 && xInput != player.facingDir){
            stateMachine.ChangeState(player.idleState);
        }
        //解决墙体下方无墙体也会滑墙状态问题
        if(!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);

        if(yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, Mathf.Lerp(rb.velocity.y, -player.wallSlideSpeed, 0.1f));

        if(player.IsGroundDetected()){
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
