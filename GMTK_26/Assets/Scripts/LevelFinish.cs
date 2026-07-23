using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.gameObject.layer == 7)
        {
            GlobalVariables.PriviousScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("Level Finished");
            Debug.Log("Level finished");
        }
    }
}
