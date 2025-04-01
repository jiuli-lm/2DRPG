using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KL : Enemy
{
    #region 骷髅敌人状态
    
    public KLIdleState idleState{get; private set;}
    public KLMoveState moveState{get; private set;}
    public KLBattleState battleState{get; private set;}
    public KLAttackState attackState{get; private set;}

    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new KLIdleState(this, stateMachine, "Idle", this);
        moveState = new KLMoveState(this, stateMachine, "Move", this);
        battleState = new KLBattleState(this, stateMachine, "Move", this);
        attackState = new KLAttackState(this, stateMachine, "Attack", this);
    }
    
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    
    protected override void Update()
    {
        base.Update();
    }


}
