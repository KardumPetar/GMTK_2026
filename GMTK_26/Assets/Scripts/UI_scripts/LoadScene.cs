using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadPreviousScene() {
        SceneManager.LoadScene(GlobalVariables.PriviousScene);
    }
    public void LoadNextScene() {
        SceneManager.LoadScene(GlobalVariables.PriviousScene + 1);
    }
    public void LoadSceneByName(string sceneName) {

        SceneManager.LoadScene(sceneName);
    }
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void doExitGame() {
        Application.Quit();
    }
    public void LoadSceneByNameDelay(string sceneName) {
        StartCoroutine(Load(sceneName));
    }
    private IEnumerator Load(string sceneName) {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(sceneName);
    }
}
