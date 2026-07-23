using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill:MonoBehaviour
{
    protected string triger = "1";
    public virtual void SkillFunction(){
        print("Skill: " + triger);
    }
    public virtual void SkillTirgerFunction() {
        if (Input.GetKey(triger)){
            SkillFunction();
        } 
    }
    private void FixedUpdate(){
        SkillTirgerFunction();
    }
}
