using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public enum SwordType
{
    Regular, //常规
    Bounce,  //反弹
    Pierce,  //穿透
    Spin     //旋转
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Bounce;
    
    [Header("反弹技能")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    
    [Header("穿刺技能")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("旋转技能")] 
    [SerializeField] private float hitCooldown;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    
    [Header("技能信息")]
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;
    
    [Header("Aim daots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
    }

    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x*launchForce.x, AimDirection().normalized.y*launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetweenDots);
            }
        }
    }
    
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true,bounceAmount);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);
        
        newSwordScript.SetupSword(finalDir,swordGravity,player);
        
        player.AssignNewSword(newSword);
        
        DotsActive(false); 
    }
    
    
    #region 瞄准线
    
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }
    
    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }
    
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab,player.transform.position,Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }
    
    private Vector2 DotsPosition(float t)
    {
        //物理公式 斜抛 1/2 gt^2
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x*launchForce.x, 
            AimDirection().normalized.y*launchForce.y)*t + .5f*(Physics2D.gravity * swordGravity) * (t * t);
        
        return position;
    } 
    
    #endregion

}
