using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    public string skillName = "Skill";
    public int cost = 30;
    public TMPro.TextMeshProUGUI textName;
    public TMPro.TextMeshProUGUI textCost;
    
    private void Start() {
        textName.text = skillName;
        textCost.text = cost + " s";
    }
    public void BuySkill() {
        print(skill.gameObject.name);
        if (CountDown.clockTime >= cost) {
            CountDown.clockTime -= cost;
            skill.enabled = true;
        }   
    }
}
