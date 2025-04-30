using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int totalEnemies = 5;

    [Header("Manager References")]
    [SerializeField] private EnemyCounter enemyCounter;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private PlayerManager playerManager;

    private bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // èâä˙ê›íË
        enemyCounter.Initialize(totalEnemies);
        playerManager.DisableControl();
        gameTimer.ResetTimer();

        uiManager.StartReadyCountdown(OnGameStart);
    }

    private void Update()
    {
        if (isGameActive)
        {
            uiManager.UpdateTimerUI(gameTimer.GetFormattedTime());
        }
    }

    private void OnGameStart()
    {
        isGameActive = true;
        gameTimer.StartTimer();
        playerManager.EnableControl();
    }

    public void EnemyDefeated()
    {
        enemyCounter.Decrease();

        if (enemyCounter.GetRemaining() == 0 && isGameActive)
        {
            EndGame();
        }
    }

    public void VillagerDefeated()
    {
        if (!isGameActive) return;

        isGameActive = false;
        playerManager.DisableControl();
        sceneController.PauseGame();

        sceneController.LoadSceneWithDelay("GameOverScene", 2f);
    }

    public void EndGame()
    {
        isGameActive = false;
        gameTimer.StopTimer();
        playerManager.DisableControl();

        uiManager.ShowGameClearText();
        CheckAndSaveBestTime();

        sceneController.PauseGame();
        sceneController.LoadSceneWithDelay("ClearScene", 3f);
    }

    private void CheckAndSaveBestTime()
    {
        float finalTime = gameTimer.GetTime();
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (finalTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", finalTime);
            PlayerPrefs.Save();
        }
    }

    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    public int GetEnemiesRemaining()
    {
        return enemyCounter.GetRemaining();
    }

    public float GetFinalTime()
    {
        return gameTimer.GetTime();
    }
}
