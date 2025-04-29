using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;
    
    [Header("水晶幻象")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    [Header("水晶单体")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked{get;private set;}
    
    [Header("水晶爆炸")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    
    [Header("水晶移动")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    
    [Header("水晶分裂")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCloneInstead);
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosive);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
        
    }
    
    private void UnlockCloneInstead()
    {
        if (unlockCloneInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }
    private void UnlockExplosive()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }
    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
            canUseMultiStacks = true;
    }

    

    public override void UseSkill()
    {
        base.UseSkill();
        if(CanUseMultiCrystal())
            return;
        
        if (currentCrystal == null)
            CreateCrystal();
        
        
        else
        {
            if (canMoveToEnemy)
                return;
            
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.Instance.clone.CreateClone(currentCrystal.transform,Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
            
            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
        }
        
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
            
        currentCrystalScript.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnemy(currentCrystal.transform),player);
    }

    public void CurrentCrystalChooseRandomEnemy() =>
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {

            if (crystalLeft.Count > 0)
            {
                if(crystalLeft.Count== amountOfStacks)
                    Invoke("ResetAbility",useTimeWindow);
                
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalSkillController>().
                    SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnemy(newCrystal.transform),player);
                
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefileCrystal();
                }
                            
                return true;
            }
        }

        return false;
    }
    

    private void RefileCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        cooldownTimer = multiStackCooldown;
        RefileCrystal();
    }
    
    
}
