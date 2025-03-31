using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 敌人属性

    #endregion

    #region 敌人组件
    public Rigidbody2D rb {get; private set;}
    public Animator anim {get; private set;}
    public EnemyStateMachine stateMachine{get; private set;}

    #endregion

    #region 敌人状态

    #endregion

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

}
