using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.skill.clone.CreateCloneOnDashStart();
        stateTimer = player.dashDuration; // 设置冲刺状态的持续时间
    }
    public override void Update()
    {
        base.Update();
        
        if(!player.IsGroundDetected() && player.IsWallDetected()){
            stateMachine.ChangeState(player.wallSlide);
        }


        player.SetVelocity(player.dashSpeed * player.dashDir,0); // 设置冲刺速度

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState); // 冲刺结束后切换到Idle状态
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.skill.clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y); // 冲刺结束后将水平速度设置为0
    }
    
}
