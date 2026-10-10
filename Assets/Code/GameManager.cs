
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public enum GameState
{
    Countdown,
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

    [Header("Starting Countdown")]
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private int countdownDuration = 3;

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
        Time.timeScale = 1f;
        CurrentState = GameState.Countdown;
    }

    private void OnEnable()
    {
        if (countdownTimer != null)
            countdownTimer.OnTimerFinished += EggWins;

        if (eggHealth != null)
            eggHealth.OnEggDied += ChickenWins;
    }

    private void OnDisable()
    {
        if (countdownTimer != null)
            countdownTimer.OnTimerFinished -= EggWins;

        if (eggHealth != null)
            eggHealth.OnEggDied -= ChickenWins;
    }

    private void Start()
    {
        countdownTimer.ResetTimer();
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Freeze gameplay.
        Time.timeScale = 0f;

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        for (int i = countdownDuration; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            // Wait using real time, even while the game is frozen.
            yield return new WaitForSecondsRealtime(1f);
        }

        if (countdownText != null)
            countdownText.text = "GO!";

        yield return new WaitForSecondsRealtime(0.5f);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        // Unfreeze gameplay and start the round.
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        countdownTimer.StartTimer();
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
