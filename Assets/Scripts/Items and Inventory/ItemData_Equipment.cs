using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,//武器
    Armor, //护甲
    Amulet,//护符
    Flask,//药瓶
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{

    public float itemCooldown;
    public ItemEffect[] itemEffects;
    public EquipmentType equipmentType;
    
    [Header("主要属性")]
    public int strength;//力量
    public int agility;//敏捷
    public int intelligence;//智力
    public int vitality;//耐力
    
    [Header("攻击属性")]
    public int damage;
    public int critChance;//暴击率
    public int critPower;//暴击伤害
    
    
    [Header("防御属性")]
    public int maxHp;
    public int armor;//护甲
    public int evasion;//闪避
    public int magicResistance;//魔法抗性
    
    [Header("元素属性")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;
    
    [Header("工艺品需求")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;
    
    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        
        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);
        
        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
        
        
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);
       
        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        AddItemDescription(strength,"Strength");
        AddItemDescription(agility,"Agility");
        AddItemDescription(intelligence,"Intelligence");
        AddItemDescription(vitality,"Vitality");
        AddItemDescription(damage,"Damage");
        AddItemDescription(critChance,"Crit Chance");
        AddItemDescription(critPower,"Crit Power");
        AddItemDescription(maxHp,"Max HP");
        AddItemDescription(evasion,"Evasion");
        AddItemDescription(armor,"Armor");
        AddItemDescription(magicResistance,"Magic Resistance");
        AddItemDescription(fireDamage,"Fire Damage");
        AddItemDescription(iceDamage,"Ice Damage");
        AddItemDescription(lightingDamage,"Lighting Damage");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine(itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }
        
        if (descriptionLength < 5)
        {
            for (int i = 0; i <5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        return sb.ToString();
    }
    
    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();
            if(_value > 0)
                sb.Append("+" + _value + "  " + _name);
            descriptionLength++;
        }
    }
    
}
