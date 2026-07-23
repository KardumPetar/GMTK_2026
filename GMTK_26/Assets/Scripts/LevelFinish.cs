using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("Level finished");
        }
    }
}
