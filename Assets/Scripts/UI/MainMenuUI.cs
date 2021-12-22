using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private string newGameSceneName = GlobalFields.dialogueSceneName;

    public void LoadNewGameScene()
    {
        SceneManager.LoadScene(newGameSceneName, LoadSceneMode.Single);
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }
}
