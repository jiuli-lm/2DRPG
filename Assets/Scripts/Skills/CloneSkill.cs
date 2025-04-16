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
    
    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration,canAttack,_offset);
    }
    
    public override void UseSkill()
    {
        base.UseSkill();
    }
}
