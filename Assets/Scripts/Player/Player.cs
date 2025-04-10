using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player: Entity
{
    [Header("攻击细节")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    
    public bool isBusy {get; private set;}//攻击僵直
    [Header("角色属性")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float wallSlideSpeed = 2f;
    [Header("冲刺属性")]
    [SerializeField]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set; }

    public SkillManager skill { get; private set; }
    
    public PlayerStateMachine stateMachine { get;  private set; }
    
    #region 角色状态

    public PlayerIdleState idleState { get;  private set; }
    public PlayerMoveState moveState { get;  private set; }
    public PlayerJumpState jumpState { get;  private set; }
    public PlayerAirState airState { get;  private set; }
    public PlayerDashState dashState { get;  private set; }
    public PlayerWallSlideState wallSlide{get; private set;}
    public PlayerWallJumpState wallJump{get; private set;}
    public PlayerPrimaryAttackState primaryAttack{get; private set;}
    public PlayerCounterAttackState counterAttack{get; private set;}
    
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlider");
        wallJump = new PlayerWallJumpState(this, stateMachine,"Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        skill = SkillManager.Instance;
        stateMachine.Initialize(idleState);
        
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDashInput();

    }
    public IEnumerator BusyFor(float seconds){
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    private void CheckDashInput()
    {
        if(IsWallDetected()) return;
        
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse1)) 
            && SkillManager.Instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir; // 如果没有输入方向，则使用当前朝向
            
            stateMachine.ChangeState(dashState);
        }
    }
}