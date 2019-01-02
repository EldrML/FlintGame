using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootSceneAnimationHelper : MonoBehaviour
{
    public SceneChanger gameManager;

    public void GoToStartGameMenu()
    {
        gameManager.GoToMainMenu();
    }
}
