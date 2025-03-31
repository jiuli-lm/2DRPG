using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLIdleState : EnemyState
{
    private Enemy_KL enemy;
    
    public KLIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
