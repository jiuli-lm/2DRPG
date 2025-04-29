using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("克隆信息")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab; // 克隆体预制体
    [SerializeField] private float cloneDuration; // 克隆体持续时间
    [Space] 
    
    [Header("克隆攻击")] 
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockedButton;
    [SerializeField] private float cloneAttackMultiplier; // 克隆体攻击倍率
    [SerializeField] private bool canAttack; // 克隆体是否可以攻击
    
    [Header("积极性克隆？")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockedButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect{get;private set;}
    
    
    [Header("多克隆")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockedButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("水晶替代克隆")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockedButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }


    #region 技能解锁

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockedButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }
    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockedButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }
    private void UnlockMultipleClone()
    {
        if (multipleUnlockedButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }
    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockedButton.unlocked)
            crystalInsteadOfClone = true;
        
    }

    #endregion
    
    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player,attackMultiplier);
    }
    
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
            StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(1f * player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
    
}
