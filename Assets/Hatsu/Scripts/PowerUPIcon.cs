using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUPIcon : MonoBehaviour
{
    private Image image;
    private bool animate = false;

    private float counter = 0;
    private float intervalCounter;
    private float flashInterval = 0.1f;
    private float duration = 1.5f;
    [SerializeField] Sprite[] sprites;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    void Update()
    {
        if (animate)
        {
            counter += Time.deltaTime;
            intervalCounter += Time.deltaTime;
            if(intervalCounter >= flashInterval)
            {
                image.enabled = !image.enabled;
                intervalCounter = 0;
            }

            if(counter >= duration)
            {
                image.enabled = false;
                animate = false;
                counter = 0;
                intervalCounter = 0;
            }
        }
    }
    public void UpdateIcon(PowerupInventory.PowerupTypes powerup)
    {
        Debug.Log(powerup);
        image.enabled = true;
        switch (powerup)
        {
            case (PowerupInventory.PowerupTypes.IncreaseSeedVelocity):
                image.sprite = sprites[0];
                break;
            case (PowerupInventory.PowerupTypes.Multishot) :
                image.sprite = sprites[1];
                 break;
            case (PowerupInventory.PowerupTypes.EggReverseControls) :
                image.sprite = sprites[2];
                 break;
            case (PowerupInventory.PowerupTypes.SpeedUpEgg) :
                image.sprite = sprites[3];
                break;
            case (PowerupInventory.PowerupTypes.WormHole) :
                image.sprite = sprites[4];
                break;
            case (PowerupInventory.PowerupTypes.SummonDecoy) :
                image.sprite = sprites[5];
                break;
            case (PowerupInventory.PowerupTypes.ExpandOrbital) :
                image.sprite = sprites[6];
                break;
            case (PowerupInventory.PowerupTypes.GoInvisible) :
                image.sprite = sprites[7];
                break;
                
        }
    }

    public void AnimateIcon()
    {
        animate = true;
    }
}
