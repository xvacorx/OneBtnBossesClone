using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public TMP_Text currentTimeText;
    public TMP_Text bestTimeText;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(false);

        if (bestTimeText != null)
            bestTimeText.gameObject.SetActive(false);
    }

    public void Victory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
            GameTimer gameTimer = FindObjectOfType<GameTimer>();
            gameTimer.StopTimer();
            float finalTime = gameTimer.GetElapsedTime();
            currentTimeText.text = $"Tiempo: {finalTime:F2} segundos";

            float bestTime = PlayerPrefs.GetFloat("BestTime", Mathf.Infinity);
            if (finalTime < bestTime)
            {
                bestTime = finalTime;
                PlayerPrefs.SetFloat("BestTime", bestTime);
                bestTimeText.text = "¡Nuevo Mejor Tiempo!";
            }
            else
            {
                bestTimeText.text = $"Mejor Tiempo: {bestTime:F2} segundos";
            }
            bestTimeText.gameObject.SetActive(true);
        }
    }

    public void Defeat()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0f;
            GameTimer gameTimer = FindObjectOfType<GameTimer>();
            gameTimer.StopTimer();
            bestTimeText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (bestTimeText != null)
        {
            bestTimeText.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}