using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLDeadState : EnemyState
{
    private Enemy_KL enemy;
    
    public KLDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_KL _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer>0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .1f;
    }

    
}
