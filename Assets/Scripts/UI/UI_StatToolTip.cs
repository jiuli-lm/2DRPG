using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    public void ShowStatToolTip(string description)
    {
        descriptionText.text = description;
        gameObject.SetActive(true);
    }
    
    public void HideStatToolTip()
    {
        descriptionText.text = "";
        gameObject.SetActive(false);
    }
    
}
