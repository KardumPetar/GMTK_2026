using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] int maxHP = 1;
    private int HP;
    private void Start() {
        HP = maxHP;
    }
    public void Hurt(int amount) {
        HP -= amount;
        
        if(HP <= 0) {
            Destroy(gameObject);
        }
    }
}
