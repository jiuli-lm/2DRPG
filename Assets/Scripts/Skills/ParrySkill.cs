using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("反击")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked{get;private set;}
    
    [Header("反击恢复")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f,1f)]
    [SerializeField] private float restoreHealthPercent;
    public bool restoreUnlocked{get;private set;}
    
    [Header("反击镜像")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked{get;private set;}

    protected override void Start()
    {
        //这里已经出现了问题 解锁先比技能槽UI赋值先发生 导致事件没有正常触发
        //需要点击两次按钮才能触发
        //已经将ui和button放到Awake里
        
        //后面已经修改 只放button 以及ui脚本 利用awake
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    public override void UseSkill()
    {   

        base.UseSkill();
        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercent);
            player.stats.IncreaseHealthBy(restoreAmount);
        }

    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlocked)
            restoreUnlocked = true;
        
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if(parryWithMirageUnlocked)
            SkillManager.Instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
    
}
