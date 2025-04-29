using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked{get;private set;}
    
    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot clonedOnDashUnlockButton;
    public bool clonedOnDashUnlocked{get;private set;}
    
    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
        
    public bool cloneOnArrivalUnlocked{get;private set;}

    
    
    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        //这里建议用Button的onClick事件来绑定函数
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        clonedOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockClonedOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockClonedOnDash()
    {
        if(clonedOnDashUnlockButton.unlocked)
            clonedOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    
    public void CloneOnDash()
    {
        if (clonedOnDashUnlocked)
            SkillManager.Instance.clone.CreateClone(player.transform, Vector3.zero);
        
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.Instance.clone.CreateClone(player.transform, Vector3.zero);
    }
    
}
