using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    public string skillID = "";
    public string skillName = "Skill";
    public int cost = 30;
    public TMPro.TextMeshProUGUI textName;
    public TMPro.TextMeshProUGUI textCost;

    private string[] playerMovementSkils = {"right_move", "left_move", "jump", "run", "double_jump", "crouch", "teleport"};
    private string[] gunSkils = { "fixed", "mouse" };


    private void Awake()
    {
        if (playerMovementSkils.Contains(skillID))
        {
            skill = FindFirstObjectByType<PlayerMovement>(FindObjectsInactive.Include);
        }
        else if (gunSkils.Contains(skillID))
        {
            skill = FindFirstObjectByType<Gun_Skill>(FindObjectsInactive.Include);
        }
    }


    private void Start() {
        textName.text = skillName;
        textCost.text = cost + " s";
    }
    public void BuySkill() {
        
        if (CountDown.clockTime >= cost) {
            CountDown.clockTime -= cost;
            skill.enabled = true;
            if (skillID != "") {
                skill.Allow(skillID);
            }
        }        
    }

}
