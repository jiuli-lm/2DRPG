using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    //敌人当前状态
    public EnemyState currentState {get; protected set;}
    //状态初始化
    public void Initialize(EnemyState _startState){
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState){
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

}
