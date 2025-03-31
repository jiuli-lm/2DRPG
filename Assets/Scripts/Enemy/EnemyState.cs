using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EnemyStateMachine
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemy ,EnemyStateMachine _stateMachine, string _animBoolName){
        enemy = _enemy;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }
    public virtual void Update(){
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter(){
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName,true);
    }
    public virtual void Exit(){
        enemy.anim.SetBool(animBoolName,false);
    }

}
