using JetBrains.Annotations;
using UnityEngine;

public class PowerupInventory : MonoBehaviour
{
    public enum PowerupTypes
    {
        None,
        
        // chicken powerups
        IncreaseSeedVelocity,
        Multishot,
        EggReverseControls,

        // egg powerups
        SpeedUpEgg,
        WormHole,
        SummonDecoy,
        ExpandOrbital,
        GoInvisible,

    }

    [SerializeField] private PowerupTypes currentPowerup = PowerupTypes.None; // default empty
    [SerializeField] PowerUPIcon powerupIcon;

    public bool HasPowerup()
    {
        return currentPowerup != PowerupTypes.None;
    }

    public PowerupTypes GetCurrentPowerup()
    {
        return currentPowerup;
    }

    public bool AddPowerup(PowerupTypes powerup)
    {
        // replace current held powerup
        currentPowerup = powerup;
        powerupIcon.UpdateIcon(powerup);
        
        return true;
    }

    // use powerup
    public PowerupTypes UseInventoryPowerup()
    {
        if (!HasPowerup()) return PowerupTypes.None;

        PowerupTypes powerToUse = currentPowerup; // set it to be used
        currentPowerup = PowerupTypes.None; // set the inventory to empty
        powerupIcon.AnimateIcon();

        return powerToUse;
    }

}
