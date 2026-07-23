using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveL_Skill : Skill
{
    public string Skill_triger = "d";
    public float speed = 100f;
    Rigidbody2D rb;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        triger = Skill_triger;
    }
    public override void SkillFunction(){
        rb.velocity = new Vector2(1, rb.velocity.y) * speed * Time.fixedDeltaTime;
    }
}
