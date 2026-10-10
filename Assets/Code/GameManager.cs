
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    [Header("Pause Menu")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;

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

        Time.timeScale = 0f;
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
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (countdownTimer != null)
            countdownTimer.ResetTimer();

        StartCoroutine(StartCountdown());
    }
    
    private IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        for (int i = countdownDuration; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            yield return new WaitForSecondsRealtime(1f);
        }

        if (countdownText != null)
            countdownText.text = "GO!";

        yield return new WaitForSecondsRealtime(0.5f);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        if (countdownTimer != null)
            countdownTimer.StartTimer();
    }
    
    private void Update()
    {
        bool keyboardPause =
            Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame;

        bool controllerPause = false;

        // Check every connected controller.
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.startButton.wasPressedThisFrame)
            {
                controllerPause = true;
                break;
            }
        }

        if (keyboardPause || controllerPause)
            TogglePause();
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
        if (countdownTimer != null)
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

        if (countdownTimer != null)
            countdownTimer.PauseTimer();

        if (pausePanel != null)
            pausePanel.SetActive(true);

        // Select Resume for controller navigation.
        if (resumeButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(
                resumeButton.gameObject);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        CurrentState = GameState.Playing;

        // Hide pause menu.
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 1f;

        if (countdownTimer != null)
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
