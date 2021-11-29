using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Load(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
        
        // If game was paused/restarted, ensure timeScale is reset
        Time.timeScale = 1;
    }

    public void Quit() 
    {
        Application.Quit();
    }
}
