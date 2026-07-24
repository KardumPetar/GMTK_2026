using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectalScript : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float age;
    void Update()
    {
        age += Time.deltaTime;
        if(age > lifeTime) {
            Destroy(gameObject);
        }
    }
}
