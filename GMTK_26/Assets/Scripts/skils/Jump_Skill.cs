using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Skill : Skill
{
    public string Skill_triger = "w";
    public float speed = 100f;
    Rigidbody2D rb;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        triger = Skill_triger;
    }
    public override void SkillFunction(){
        rb.AddForce(new Vector2(rb.velocity.x, 1) * speed);
    }
    public override void SkillTirgerFunction() {
        if (Input.GetKeyDown(triger)) {
            SkillFunction();
        }
    }
}
