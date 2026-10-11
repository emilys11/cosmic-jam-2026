using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Light spotLight;
    [SerializeField] GameObject pauseMenuSticks;
    [SerializeField] PauseSticks[] pauseSticks;
    void Awake()
    {
        spotLight.enabled = false;
        pauseMenuSticks.SetActive(false);
        // GetComponent<Canvas>().enabled = false;
    }
    public void ManagePauseMenu(bool enable)
    {
        pauseMenuSticks.SetActive(enable);
        // GetComponent<Canvas>().enabled = enable;
        if (enable == false)
        {
            for(int i = 0; i < pauseSticks.Length; i++)
            {
                pauseSticks[i].Reset();
            }
        }
        else
        {
            for(int i = 0; i < pauseSticks.Length; i++)
            {
                pauseSticks[i].startAnimation();
            }
        }
    }

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
