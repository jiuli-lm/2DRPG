using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLGroundedState : EnemyState
{
    protected Enemy_KL enemy;
    protected Transform player;
    public KLGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position,player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
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
}
