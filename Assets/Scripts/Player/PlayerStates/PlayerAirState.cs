using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override  void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        //此处解决滑墙时朝向问题 角色在滑墙状态时按了相反方向转换为空中状态，这时角色的面向方向facingDir应该有个翻转，但这一步还没进行。
        //在空中状态里最先做的操作是检测墙体，这时候角色面向方向没变还是保持面向墙体，所以又检测到墙壁转换到滑墙状态，变成了两个状态间转换的死循环
        if(xInput != 0){
            player.SetVelocity(player.moveSpeed * .8f * xInput,rb.velocity.y);
        }
        if(player.IsWallDetected()){
            stateMachine.ChangeState(player.wallSlide);
        }

        if(player.IsGroundDetected()){
            stateMachine.ChangeState(player.idleState);
        }
        // if(xInput != 0){
        //     player.SetVelocity(player.moveSpeed * .8f * xInput,rb.velocity.y);
        // }
    }
    
}
