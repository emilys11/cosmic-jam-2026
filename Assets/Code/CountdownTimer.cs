
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [Header("timer settings")]
    [SerializeField] private float duration = 60f;
    [SerializeField] private bool startAutomatically = true; //for testing purposes

    [Header("UI")]
    [SerializeField] private Slider timerSlider;

    private float remainingTime;
    private bool isRunning;
    private bool hasFinished;


    public event Action OnTimerFinished;

    public float RemainingTime => remainingTime;

    private void Start()
    {
        ResetTimer();

        if (startAutomatically)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (!isRunning)
            return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);

        if (timerSlider != null)
        {
            timerSlider.value = duration > 0f ? remainingTime / duration : 0f;
        }

        if (remainingTime <= 0f)
        {
            FinishTimer();
        }
    }

    public void StartTimer()
    {
        if (hasFinished || remainingTime <= 0f)
            return;

        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        remainingTime = Mathf.Max(duration, 0f);
        isRunning = false;
        hasFinished = false;

        if (timerSlider != null)
        {
            timerSlider.minValue = 0f;
            timerSlider.maxValue = 1f;
            timerSlider.value = duration > 0f ? 1f : 0f;
        }
    }

    private void FinishTimer()
    {
        if (hasFinished)
            return;

        hasFinished = true;
        isRunning = false;

        OnTimerFinished?.Invoke();
    }
}
