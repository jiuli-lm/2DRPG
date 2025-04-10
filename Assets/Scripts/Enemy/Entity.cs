using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("击退信息")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float konockbackDuration;
    protected bool isKnocked;
    
    
    [Header("碰撞属性")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public Transform attackCheck;
    public float attackCheckRidius;
    
    public int facingDir{get; private set;} = 1;
    private bool facingRight = true;
    
    #region 角色组件
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    public EntityFX fx {get; private set;}
    
    #endregion

    protected virtual void Awake(){
        
    }

    protected virtual void Start(){
        fx = GetComponentInChildren<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update(){
        
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockDirection");
        
        Debug.Log($"{gameObject.name} 受伤了");
    }

    protected virtual IEnumerator HitKnockDirection()
    {
        isKnocked = true;
        
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        
        yield return new WaitForSeconds(konockbackDuration);
        isKnocked = false;
    }
    

    #region 碰撞检测
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRidius);
    }
    #endregion

    #region 翻转
    public virtual void Filp()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipController(float x)
    {
        if(x > 0 && !facingRight){
            Filp();
        }
        else if(x < 0 && facingRight){
            Filp(); 
        }
    }
    #endregion

    #region 速度
    //设置速度0
    public void SetZeroVelocity()
    {
        if(isKnocked) return;
        rb.velocity = new Vector2(0,0);
    }
    //传递刚体的速度
    public void SetVelocity(float x_velocity, float y_velocity)
    {
        if(isKnocked) return;
        
        rb.velocity = new Vector2(x_velocity, y_velocity);
        FlipController(x_velocity);
    }
    #endregion
    
}
