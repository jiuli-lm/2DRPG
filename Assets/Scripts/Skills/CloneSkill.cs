using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("克隆信息")]
    [SerializeField] private GameObject clonePrefab; // 克隆体预制体
    [SerializeField] private float cloneDuration; // 克隆体持续时间
    [Space]
    [SerializeField] private bool canAttack; // 克隆体是否可以攻击

    [SerializeField] private bool creatCloneOnDashStart;
    [SerializeField] private bool creatCloneOnDashOver;
    [SerializeField] private bool canCloneOnCounterAttack;
    
    [Header("克隆复制攻击信息")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    
    [Header("水晶替代克隆")]
    public bool crystalInsteadOfClone;
    
    
    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player);
    }

    public void CreateCloneOnDashStart()
    {
        if (creatCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
        
    }

    public void CreateCloneOnDashOver()
    {
        if (creatCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }
    
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1f * player.facingDir, 0)));

    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
    
}
