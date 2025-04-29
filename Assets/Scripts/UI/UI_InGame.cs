using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentSouls;
    private SkillManager skills;

    void Start()
    {
        if (playerStats != null)
            playerStats.onChangeHealthed += UpdateHealthUI;
        
        skills = SkillManager.Instance;
    }

    void Update()
    {
        currentSouls.text = PlayerManager.Instance.GetCurrency().ToString("#,#");
        
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);
        if(Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);
        if(Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);
        if(Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);
        if(Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackHoleUnlocked)
            SetCooldownOf(blackHoleImage);
        if(Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);
        
        CheckCooldownOf(dashImage,skills.dash.cooldown);
        CheckCooldownOf(parryImage,skills.parry.cooldown);
        CheckCooldownOf(crystalImage,skills.crystal.cooldown);
        CheckCooldownOf(swordImage,skills.sword.cooldown);
        CheckCooldownOf(blackHoleImage,skills.blackHole.cooldown);
        CheckCooldownOf(flaskImage,Inventory.Instance.flaskCooldown);
        
        
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown *Time.deltaTime;
    }

}
