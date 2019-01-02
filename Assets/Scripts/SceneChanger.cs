using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToNdrsSandbox()
    {
        SceneManager.LoadSceneAsync("ndrs-Sandbox");
    }

    public void GoToMainMenu(){
        SceneManager.LoadSceneAsync("StartGameMenu");
    }
}
