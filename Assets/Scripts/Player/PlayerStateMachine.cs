using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //角色的当前状态
    public PlayerState currentState{ get; private set; }
    //状态的初始化
    public void Initialize(PlayerState startstate)
    {
        currentState = startstate;
        currentState.Enter();
    }
    //改变状态
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
}
