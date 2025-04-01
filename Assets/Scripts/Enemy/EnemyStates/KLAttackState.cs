using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLAttackState : EnemyState
{
    private Enemy_KL enemy;
    
    public KLAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastAttackedTime = Time.time;
    }
}
