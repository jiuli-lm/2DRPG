using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLGroundedState : EnemyState
{
    protected Enemy_KL enemy;
    
    public KLGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
