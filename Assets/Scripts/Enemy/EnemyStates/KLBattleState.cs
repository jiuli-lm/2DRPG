using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLBattleState : EnemyState
{
    private Transform player;
    private Enemy_KL enemy;

    private int moveDir;
    
    public KLBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    
    
    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(stateTimer<0)
                stateMachine.ChangeState(enemy.idleState);
        }
        
        
        if(player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if(player.position.x < enemy.transform.position.x)
            moveDir = -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Enter()
    {
        base.Enter();
        
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastAttackedTime + enemy.attackCooldown)
        {
            enemy.lastAttackedTime = Time.time;
            return true;
        }

        return false;
    }
}
