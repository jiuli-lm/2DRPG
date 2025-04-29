using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("躲避")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;

    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked{get;private set;}
    
    [Header("残影躲避")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgemirageUnlocked{get;private set;}

    protected override void Start()
    {
        base.Start();
        
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if(unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.Instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirageDodge()
    {
        if(unlockMirageDodgeButton.unlocked)
            dodgemirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if(dodgemirageUnlocked)
            SkillManager.Instance.clone.CreateClone(player.transform,new Vector3(2 * player.facingDir,0));
    }
    
    
}
