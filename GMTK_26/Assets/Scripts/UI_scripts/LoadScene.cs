using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadPreviousScene() {
        SceneManager.LoadScene(GlobalVariables.PriviousScene);
    }
    public void LoadSceneByName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
