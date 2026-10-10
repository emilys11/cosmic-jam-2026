
using UnityEngine;
using UnityEngine.UI;

public class EggLife : MonoBehaviour
{
    
    [SerializeField] float flashDuration;
    [SerializeField] float flashInterval;
    private Image image;
    private float counter;
    private float intervalCounter;
    private bool playEffect;
    private bool active;
    void Awake()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (playEffect)
        {
            counter += Time.deltaTime;
            intervalCounter += Time.deltaTime;
            if(intervalCounter >= flashInterval)
            {
                image.enabled = !image.enabled;
                intervalCounter = 0;
            }

            if(counter >= flashDuration)
            {
                gameObject.SetActive(false);
                image.enabled = true;
                playEffect = false;
                counter = 0;
                intervalCounter = 0;
            }


        }
    }

    public void BreakEgg()
    {
        playEffect = true;
    }
}
