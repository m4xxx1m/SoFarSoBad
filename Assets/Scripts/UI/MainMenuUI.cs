using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private string newGameSceneName = GlobalFields.startCutSceneName;

    [SerializeField] private Text text;
    [SerializeField] private Image newGameImg;
    [SerializeField] private Image exitImg;

    public void LoadNewGameScene()
    {
        StartCoroutine(FadeImage());
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }

    IEnumerator FadeImage()
    {
        Color textColor = text.color;
        Color newGameColor = newGameImg.color;
        Color exitColor = exitImg.color;
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            textColor.a = i;
            newGameColor.a = i;
            exitColor.a = i;
            text.color = textColor;
            newGameImg.color = newGameColor;
            exitImg.color = exitColor;
            yield return null;
        }
        SceneManager.LoadScene(newGameSceneName, LoadSceneMode.Single);
    }
}
