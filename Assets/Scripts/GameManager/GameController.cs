using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private string victoryPanelName;
    [SerializeField] private string defeatPanelName;
    [SerializeField] private string currentTimeTextName;
    [SerializeField] private string bestTimeTextName;

    private GameObject victoryPanel;
    private GameObject defeatPanel;
    private TMP_Text currentTimeText;
    private TMP_Text bestTimeText;
    private string currentSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        InitializeUIElements();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        victoryPanel = GameObject.Find(victoryPanelName);
        defeatPanel = GameObject.Find(defeatPanelName);
        GameObject currentTime = GameObject.Find(currentTimeTextName);
        GameObject bestTime = GameObject.Find(bestTimeTextName);

        if (currentTime != null) currentTimeText = currentTime.GetComponent<TMP_Text>();
        if (bestTime != null) bestTimeText = bestTime.GetComponent<TMP_Text>();

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (bestTimeText != null) bestTimeText.gameObject.SetActive(false);
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

            currentTimeText.text = $"Time: {finalTime:F2} s.";

            string bestTimeKey = $"{currentSceneName}_BestTime";
            float bestTime = PlayerPrefs.GetFloat(bestTimeKey, Mathf.Infinity);

            if (finalTime < bestTime)
            {
                bestTime = finalTime;
                PlayerPrefs.SetFloat(bestTimeKey, bestTime);
                bestTimeText.text = "New best time!";
            }
            else
            {
                bestTimeText.text = $"Best time: {bestTime:F2} s.";
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

            if (bestTimeText != null) bestTimeText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (bestTimeText != null) bestTimeText.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
