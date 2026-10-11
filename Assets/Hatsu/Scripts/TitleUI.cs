
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string transitionScene = "TransitionLevel";

    [Header("References")]
    [SerializeField] private Image blackBackground;
    [SerializeField] private Button playButton;

    [Header("Fade Settings")]
    [SerializeField] private float fadeTime = 1f;

    private bool fadeStart;
    private float transparency;
    private float fadeSpeed;

    private void Awake()
    {
        Color color = new Color(0, 0, 0, 0);
        blackBackground.color = color;
        blackBackground.enabled = false;

        fadeSpeed = fadeTime > 0f ? 1f / fadeTime : 1f;
    }

    private IEnumerator Start()
    {
        // Wait one frame for Unity's UI to initialize.
        yield return null;

        if (playButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(
                playButton.gameObject);
        }
    }

    private void Update()
    {
        if (!fadeStart)
            return;

        transparency += fadeSpeed * Time.deltaTime;

        Color color = blackBackground.color;
        color.a = Mathf.Clamp01(transparency);
        blackBackground.color = color;

        if (transparency >= 1f)
        {
            fadeStart = false;
            SwitchScene();
        }
    }

    public void Play()
    {
        if (fadeStart)
            return;

        blackBackground.enabled = true;
        fadeStart = true;
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(transitionScene);
    }

    public void Credits()
    {
        // Your credits logic here.
    }

    public void Exit()
    {
        Application.Quit();
    }
}
