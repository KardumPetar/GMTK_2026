using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    public static CountDown Instance { get; private set; }
    public static float startClockTime = 300f;
    public static float clockTime = 300f;
    public bool clockRunning = false;
    public TMPro.TextMeshProUGUI Text;

    public float timeFlowMultiplier;

    private void Awake() {
        clockTime = startClockTime;
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
        timeFlowMultiplier = 1;
    }

    void FixedUpdate()
    {
        if (clockRunning) { 
            clockTime -= timeFlowMultiplier * Time.fixedDeltaTime;
            //Debug.Log(timeFlowMultiplier);
            timeFlowMultiplier = 1;
        }

        if(clockTime < 0) {
            GlobalVariables.PriviousScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("GameOver");
        }
        else {
            string minute = Mathf.Floor(clockTime / 60).ToString();
            if (minute.Length == 1) {
                minute = "0" + minute;
            }
            string sekunde = Mathf.Floor(clockTime % 60).ToString();
            if (sekunde.Length == 1) {
                sekunde = "0" + sekunde;
            }
            Text.text = minute+ ":" + sekunde;
        }
  
    }
    public void SetClockRunning(bool val) {
        clockRunning = val;
    }
}
