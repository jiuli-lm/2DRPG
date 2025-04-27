using UnityEngine;

public enum ItemType
{
    Material, //材质
    Equipment //装备
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
 
    [Range(0,100)]
    public float dropChance;
}