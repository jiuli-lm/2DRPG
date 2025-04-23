using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private int baseValue;
    
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        
        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    
    
    public void AddModifier(int _modfier)
    {
        modifiers.Add(_modfier);
    }
    
    public void RemoveModifier(int _modfier)
    {
        modifiers.Remove(_modfier);//这里没确定是at还是remove
    }
    
}
