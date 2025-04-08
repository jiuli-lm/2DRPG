using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    protected LayerMask whatIsPlayer;
    
    [Header("震慑信息")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject countImage;
    
    [Header("敌人移动")]
    public float moveSpeed;
    public float idleTime;
    
    [Header("敌人攻击")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastAttackedTime;
    public float battleTime;
    
    public EnemyStateMachine stateMachine{get; private set;}
    
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
        
    }
    
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        countImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        countImage.SetActive(false);
    }
    
    public virtual bool CheckCanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    
    
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => 
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,50,whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
    
}
