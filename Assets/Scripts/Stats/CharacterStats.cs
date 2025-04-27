using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    
    [Header("主要属性")]
    public Stats strength;//力量
    public Stats agility;//敏捷
    public Stats intelligence;//智力
    public Stats vitality;//耐力
    
    [Header("攻击属性")]
    public Stats damage;
    public Stats critChance;//暴击率
    public Stats critPower;//暴击伤害
    
    
    [Header("防御属性")]
    public Stats maxHp;
    public Stats armor;//护甲
    public Stats evasion;//闪避
    public Stats magicResistance;//魔法抗性
    
    [Header("元素属性")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;
    
    public bool isIgnited;//是否被点燃
    public bool isChilled;//是否被冰冻
    public bool isShocked;//是否被击晕

    [SerializeField]
    private float allmentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    
    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockedDamage;
    public int currentHealth;
    
    public Action onChangeHealthed;
    public bool isDead { get; private set; }

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        
        igniteDamageTimer -= Time.deltaTime;
        
        
        if(ignitedTimer < 0)
            isIgnited = false;
        if(chilledTimer<0)
            isChilled = false;
        if(shockedTimer<0)
            isShocked = false;
        if(isIgnited)
            ApplyIgniteDamage();
        
        
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        
        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    #region 魔法伤害
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();
        
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage+intelligence.GetValue();
        
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);
        
        if(Mathf.Max(_fireDamage, _iceDamage,_lightingDamage)<=0)
            return;
        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;
        
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        if(canApplyShock)
            _targetStats.SetupShockedDamage(Mathf.RoundToInt(_lightingDamage * .2f));
        
        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = allmentsDuration;
            
            fx.IgniteFxFor(allmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;      
            chilledTimer = allmentsDuration;
            
            float slowPercentage = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage,allmentsDuration);
            fx.ChillFxFor(allmentsDuration);
        }
        
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if(GetComponent<Player>()!=null)
                    return;

                HitNearestTargetWithShockStrike();
            }

        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;
        shockedTimer = allmentsDuration;
        isShocked = _shock;
        fx.ShockFxFor(allmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if(closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab,transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockedDamage,closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        if(igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if(currentHealth<0 && !isDead)
                Die();
            
            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage)=> igniteDamage = _damage;
    public void SetupShockedDamage(int _damage)=> shockedDamage = _damage;
    
    #endregion
    
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        if (currentHealth < 0 && !isDead)
            Die();
        onChangeHealthed();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if(currentHealth>GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        if (onChangeHealthed != null)
            onChangeHealthed();
    }
    
    public virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onChangeHealthed != null)
        {
            onChangeHealthed();
        }
    }
    

    protected virtual void Die()
    {
        isDead = true;
    }

    #region 计算属性
    
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;
        
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("攻击被闪避");
            return true;
        }

        return false;
    }
    
    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }
    
    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = critPower.GetValue() + intelligence.GetValue() * .01f;
        float critDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(critDamage);
    }
    
    public int GetMaxHealthValue() => maxHp.GetValue() + vitality.GetValue() * 5;
    #endregion
}
