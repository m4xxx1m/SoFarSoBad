using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    private string mainMenuSceneName = GlobalFields.mainMenuSceneName;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayMenuPanel;
    [SerializeField] private GameObject gameOverMenuPanel;
    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private Image healthIndicator;
    [SerializeField] private Image radiationIndicator;

    private float healthIndicatorStartWidth = 1f;
    private float radiationIndicatorStartWidth = 1f;

    public float healthIndicatorWidth
    {
        get {
            return healthIndicator.rectTransform.rect.width/healthIndicatorStartWidth;
        }
        set
        {
            //if (value < 0.0f || value > 1.0f)
                //Debug.LogError("�� � ���� ����������, �� ������� � healthIndicatorWidth �������� ������ �������. ��� ������ ������ healthIndicatorWidth ������ ���� % �������� ������ � ���������� �����.");
            if (value < 0.0f)
                value = 0.0f;
            if (value > 1.0f)
                value = 1.0f;
            float targetValue = value * healthIndicatorStartWidth;
            Rect rect = healthIndicator.rectTransform.rect;
            healthIndicator.rectTransform.sizeDelta = new Vector2(targetValue, rect.height);
        }
    }
    public float radiationIndicatorWidth
    {
        get
        {
            return radiationIndicator.rectTransform.rect.width/radiationIndicatorStartWidth;
        }
        set
        {
            //if (value < 0f || value > 1f)
                //Debug.LogError("�� � ���� ����������, �� ������� � radiationIndicatorWidth �������� ������ �������. ��� ������ ������ radiationIndicatorWidth ������ ���� % �������� ������ � ���������� �����.");
            if (value < 0f)
                value = 0f;
            if (value > 1f)
                value = 1f;
            float targetValue = value * radiationIndicatorStartWidth;
            Rect rect = radiationIndicator.rectTransform.rect;
            radiationIndicator.rectTransform.sizeDelta = new Vector2(targetValue, rect.height);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        healthIndicatorStartWidth = healthIndicatorWidth;
        radiationIndicatorStartWidth = radiationIndicatorWidth;
        radiationIndicatorWidth = 0f;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        CloseGameplayMenu();
    }

    public void OpenGameplayMenu()
    {
        Time.timeScale = 1f;
        gameplayMenuPanel.SetActive(true);
        CloseMenu();
    }

    public void OpenGameOverMenu()
    {
        gameOverMenuPanel.SetActive(true);
        CloseGameplayMenu();
        CloseMenu();
    }

    public void OpenWinMenu()
    {
        winMenuPanel.SetActive(true);
        CloseGameplayMenu();
        CloseMenu();
    }

    public void CloseGameplayMenu()
    {
        gameplayMenuPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }

    public void GoToScoretable()
    {
        SceneManager.LoadScene(GlobalFields.scoretableSceneName, LoadSceneMode.Single);
    }
}

