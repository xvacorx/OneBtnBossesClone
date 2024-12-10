using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("UI Element Names")]
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
        Time.timeScale = 1f;
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        if (victoryPanel == null)
            victoryPanel = FindAndValidateGameObject(victoryPanelName, "Victory Panel");
        if (defeatPanel == null)
            defeatPanel = FindAndValidateGameObject(defeatPanelName, "Defeat Panel");
        if (currentTimeText == null)
            currentTimeText = FindAndValidateTMPText(currentTimeTextName, "Current Time Text");
        if (bestTimeText == null)
            bestTimeText = FindAndValidateTMPText(bestTimeTextName, "Best Time Text");

        HideAllPanels();
    }

    private GameObject FindAndValidateGameObject(string objectName, string description)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null) Debug.LogError($"{description} not found: {objectName}");
        return obj;
    }

    private TMP_Text FindAndValidateTMPText(string objectName, string description)
    {
        GameObject obj = FindAndValidateGameObject(objectName, description);
        return obj != null ? obj.GetComponent<TMP_Text>() : null;
    }

    private void HideAllPanels()
    {
        if (victoryPanel) victoryPanel.SetActive(false);
        if (defeatPanel) defeatPanel.SetActive(false);
        if (bestTimeText) bestTimeText.gameObject.SetActive(false);
    }

    public void Victory()
    {
        if (victoryPanel == null) return;

        victoryPanel.SetActive(true);
        Time.timeScale = 0f;

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer == null) return;

        gameTimer.StopTimer();
        float finalTime = gameTimer.GetElapsedTime();
        UpdateCurrentTime(finalTime);
        UpdateBestTime(finalTime);
    }

    private void UpdateCurrentTime(float finalTime)
    {
        if (currentTimeText)
            currentTimeText.text = $"Time: {finalTime:F2} s.";
    }

    private void UpdateBestTime(float finalTime)
    {
        if (bestTimeText == null) return;

        string bestTimeKey = $"{currentSceneName}_BestTime";
        float bestTime = PlayerPrefs.GetFloat(bestTimeKey, Mathf.Infinity);

        if (finalTime < bestTime)
        {
            PlayerPrefs.SetFloat(bestTimeKey, finalTime);
            bestTimeText.text = "New best time!";
        }
        else
        {
            bestTimeText.text = $"Best time: {bestTime:F2} s.";
        }

        bestTimeText.gameObject.SetActive(true);
    }

    public void Defeat()
    {
        if (defeatPanel == null) return;

        defeatPanel.SetActive(true);
        Time.timeScale = 0f;

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        gameTimer?.StopTimer();

        if (bestTimeText) bestTimeText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (bestTimeText) bestTimeText.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
