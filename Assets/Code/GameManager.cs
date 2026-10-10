
using System;
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

    [Header("Game Settings")]
    [SerializeField] private float roundDuration = 60f;

    [Header("Scene Names")]
    [SerializeField] private string chickenWinScene = "Chicken_Win";
    [SerializeField] private string eggWinScene = "Egg_Win";

    public GameState CurrentState { get; private set; }
    public float TimeRemaining { get; private set; }

    public bool IsPlaying => CurrentState == GameState.Playing;

    public event Action<float> OnTimeChanged;
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        TimeRemaining = roundDuration;
    }

    private void Start()
    {
        OnTimeChanged?.Invoke(TimeRemaining);
        OnStateChanged?.Invoke(CurrentState);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (!IsPlaying)
            return;

        TimeRemaining = Mathf.Max(
            0f, TimeRemaining - Time.deltaTime);

        OnTimeChanged?.Invoke(TimeRemaining);

        if (TimeRemaining <= 0f)
            EggWins();
    }

    public void ChickenWins()
    {
        if (!IsPlaying)
            return;

        FinishGame(GameState.ChickenWon, chickenWinScene);
    }

    public void EggWins()
    {
        if (!IsPlaying)
            return;

        FinishGame(GameState.EggWon, eggWinScene);
    }

    private void FinishGame(GameState result, string sceneName)
    {
        CurrentState = result;
        OnStateChanged?.Invoke(result);

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
        OnStateChanged?.Invoke(CurrentState);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        OnStateChanged?.Invoke(CurrentState);
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
