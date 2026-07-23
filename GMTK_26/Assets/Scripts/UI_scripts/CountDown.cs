using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    public static CountDown Instance { get; private set; }
    public float clockTime = 300f;
    public TMPro.TextMeshProUGUI Text;
    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    void Update()
    {
        clockTime -= Time.deltaTime;
        Text.text= Mathf.Floor(clockTime/60) + ":" + Mathf.Round(clockTime%60);

        if(clockTime < 0) {
            GlobalVariables.PriviousScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("GameOver");
        }
        else {
            Text.text = Mathf.Floor(clockTime / 60) + ":" + Mathf.Round(clockTime % 60);
        }
    }
}
