using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackHoleDuration;
    
    BlackHoleSkillController currentBlackHole;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();
        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown,blackHoleDuration);

    }

    public bool SkillCompleted()
    {
        if (!currentBlackHole)
            return false;
        
        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }
    
    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
    
}
