using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUPIcon : MonoBehaviour
{
    private Image image;
    [SerializeField] Sprite[] sprites;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    public void UpdateIcon(PowerupInventory.PowerupTypes powerup)
    {
        image.enabled = true;
        switch (powerup)
        {
            case (PowerupInventory.PowerupTypes.IncreaseSeedVelocity):
                image.sprite = sprites[0];
                break;
            case (PowerupInventory.PowerupTypes.Multishot) :
                image.sprite = sprites[1];
                 break;
            case (PowerupInventory.PowerupTypes.SpeedUpEgg) :
                image.sprite = sprites[2];
                break;
            case (PowerupInventory.PowerupTypes.WormHole) :
                image.sprite = sprites[3];
                break;
            case (PowerupInventory.PowerupTypes.SummonDecoy) :
                image.sprite = sprites[4];
                break;
            case (PowerupInventory.PowerupTypes.ExpandOrbital) :
                image.sprite = sprites[5];
                break;
            case (PowerupInventory.PowerupTypes.GoInvisible) :
                image.sprite = sprites[6];
                break;
                
        }
    }
}
