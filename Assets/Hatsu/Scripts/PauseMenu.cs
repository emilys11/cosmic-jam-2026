using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Light spotLight;
    [SerializeField] GameObject pauseMenuSticks;
    [SerializeField] PauseSticks[] pauseSticks;
    [SerializeField] private Button defaultButton;
    void Awake()
    {
        spotLight.enabled = false;
        pauseMenuSticks.SetActive(false);
        // GetComponent<Canvas>().enabled = false;
    }

    void OnEnable()
    {
        pauseMenuSticks.SetActive(true);
        for(int i = 0; i < pauseSticks.Length; i++)
        {
            pauseSticks[i].startAnimation();
        }
        if (EventSystem.current != null && defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
    }

    void OnDisable()
    {
        pauseMenuSticks.SetActive(false);
        for(int i = 0; i < pauseSticks.Length; i++)
        {
            pauseSticks[i].Reset();
        }
    }
    // public void ManagePauseMenu(bool enable)
    // {
    //     pauseMenuSticks.SetActive(enable);
    //     // GetComponent<Canvas>().enabled = enable;
    //     if (enable == false)
    //     {
    //         for(int i = 0; i < pauseSticks.Length; i++)
    //         {
    //             pauseSticks[i].Reset();
    //         }
    //     }
    //     else
    //     {
    //         for(int i = 0; i < pauseSticks.Length; i++)
    //         {
    //             pauseSticks[i].startAnimation();
    //         }
    //     }
    // }

    public void IncreaseScale(PauseSticks target)
    {
        spotLight.enabled = true;
        spotLight.gameObject.transform.LookAt(target.gameObject.transform, Vector3.up);
        target.ScaleStick(1.2f);
    }

    public void DecreaseScale(PauseSticks target)
    {
        spotLight.enabled = false;
        target.ScaleStick(1f);
    }
}
