using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 12;
    
    [Header("穿刺信息")]
    private float pierceAmount;
    
    [Header("反弹信息")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;
    
    [Header("旋转信息")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            animator.SetBool("Rotation", true);
        
        Invoke("DestroyMe", 7);
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    
    public void SetupBounce(bool _isBouncing,int _amountOfBounces)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        
        enemyTarget = new List<Transform>();
    }
    
    public void SetupSpin(bool _isSpining,float _maxTravelDistance, float _spinDuration,float _hitCooldown)
    {
        isSpinning = _isSpining;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    
    private void Update()
    {
        if(canRotate)
            transform.right = rb.velocity;
        if (isReturning)
        { 
            transform.position = Vector2.MoveTowards(transform.position,
                player.transform.position,returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
            
        }

        BounceLogic();
        SpinLogic();
        
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 1, transform.position.y),2.4f * Time.deltaTime);
                
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
                
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            
            transform.position = Vector2.MoveTowards(transform.position,
                enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {

                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                
                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count) 
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isReturning) return;
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);

        }
        SetupTargetForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    { 
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        
        player.stats.DoDamage(enemyStats);
        
        //冻结没做
        // if(player.skill.sword.timeStopUnlocked)
        //     enemy.FreezeTimerFor(freezeTimeDuration);
        
        if(player.skill.sword.timeStopUnlocked)
            enemyStats.MakeVulnerableFor(freezeTimeDuration);
        
        ItemData_Equipment equipedAmulet = Inventory.Instance?.GetEquipment(EquipmentType.Amulet);
            
        if(equipedAmulet != null)
           equipedAmulet.Effect(enemy.transform);
    }
    
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();//在此处为碰到第一个敌人停止
            return;
        }
            
        
        canRotate = false;
        cd.enabled = false;
        
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;
        
        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
