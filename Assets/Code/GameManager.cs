
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState
{
    Playing,
    Paused,
    ChickenWon,
    EggWon
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private EggHealth eggHealth;

    [Header("Scenes")]
    [SerializeField] private string chickenWinScene = "ChickenWin";
    [SerializeField] private string eggWinScene = "EggWin";

    public GameState CurrentState { get; private set; }
    public bool IsPlaying => CurrentState == GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        // Listen for the timer expiring.
        if (countdownTimer != null)
            countdownTimer.OnTimerFinished += EggWins;

        // Listen for the egg being defeated.
        if (eggHealth != null)
            eggHealth.OnEggDied += ChickenWins;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent stale event references.
        if (countdownTimer != null)
            countdownTimer.OnTimerFinished -= EggWins;

        if (eggHealth != null)
            eggHealth.OnEggDied -= ChickenWins;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void ChickenWins()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.ChickenWon;
        FinishGame(chickenWinScene);
    }

    public void EggWins()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.EggWon;
        FinishGame(eggWinScene);
    }

    private void FinishGame(string sceneName)
    {
        countdownTimer.PauseTimer();
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
            PauseGame();
        else if (CurrentState == GameState.Paused)
            ResumeGame();
    }

    public void PauseGame()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        countdownTimer.PauseTimer();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        countdownTimer.StartTimer();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Time.timeScale = 1f;
            Instance = null;
        }
    }
}
