using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player: Entity
{
    [Header("攻击细节")]
    public Vector2[] attackMovement;
    public bool isBusy {get; private set;}//攻击僵直
    [Header("角色属性")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float wallSlideSpeed = 2f;
    [Header("冲刺属性")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set; }


    #region 角色组件

    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    public PlayerStateMachine stateMachine { get;  private set; }

    #endregion
    
    #region 角色状态

    public PlayerIdleState idleState { get;  private set; }
    public PlayerMoveState moveState { get;  private set; }
    public PlayerJumpState jumpState { get;  private set; }
    public PlayerAirState airState { get;  private set; }
    public PlayerDashState dashState { get;  private set; }
    public PlayerWallSlideState wallSlide{get; private set;}
    public PlayerWallJumpState wallJump{get; private set;}
    public PlayerPrimaryAttackState primaryAttack{get; private set;}

    #endregion

    protected override void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlider");
        wallJump = new PlayerWallJumpState(this, stateMachine,"Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
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

        dashUsageTimer -= Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse1)) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown; // 重置冲刺冷却时间
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir; // 如果没有输入方向，则使用当前朝向
            }
            stateMachine.ChangeState(dashState);
        }
    }
    
    #region 速度
    //设置速度0
    public void ZeroVelocity() => rb.velocity = new Vector2(0,0);
    //传递刚体的速度
    public void SetVelocity(float x_velocity, float y_velocity)
    {
        rb.velocity = new Vector2(x_velocity, y_velocity);
        FlipController(x_velocity);
    }
    #endregion

}