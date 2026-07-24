using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] int maxHP = 1;
    [SerializeField] GameObject destroyParticleSystem;
    private GameObject newParticleSystem;
    private int HP;
    private void Start() {
        HP = maxHP;
    }
    public void Hurt(int amount) {
        HP -= amount;
        
        if(HP <= 0) {
            if (destroyParticleSystem != null) {
                newParticleSystem = Instantiate(destroyParticleSystem, GameObject.Find("Projectals").transform);
                newParticleSystem.transform.position = transform.position;
            }
            Destroy(gameObject);
        }

    }
}
