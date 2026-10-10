using System.Collections;
using UnityEngine;

public class ChickenPowerups : MonoBehaviour
{
    [SerializeField] ChickenShooting chickenShooting;
    [SerializeField] float cooldownDecreaseMultiplier = 0.8f; // TO BE TRIALED

    [SerializeField] float powerupTimer = 0f;
    private Coroutine decreaseCooldownCoroutine;
    private Coroutine multishotCoroutine;

    [SerializeField] PowerupInventory inventory;

    public void ActivateCooldownDecrease()
    {
        if (decreaseCooldownCoroutine != null) return;
        decreaseCooldownCoroutine = StartCoroutine(DecreaseCooldownTimer());
    }

    IEnumerator DecreaseCooldownTimer()
    {
        chickenShooting.ActivateCooldownDecrease(cooldownDecreaseMultiplier);
        yield return new WaitForSeconds(powerupTimer);
        chickenShooting.DeactivateCooldownDecrease();
        decreaseCooldownCoroutine = null;
    }

    public void ActivateMultishot()
    {
        if (multishotCoroutine != null) return;

        multishotCoroutine = StartCoroutine(MultishotTimer());
    }

    IEnumerator MultishotTimer()
    {
        chickenShooting.ActivateMultishot();
        yield return new WaitForSeconds(powerupTimer);
        chickenShooting.DeactivateMultishot();
        multishotCoroutine = null;
    }

    public void ActivateEggReverseControls()
    {
        // egg controller reverse controls
    }


    public void UsePowerups()
    {
        PowerupInventory.PowerupTypes type = inventory.UseInventoryPowerup();

        switch(type)
        {
            case PowerupInventory.PowerupTypes.DecreaseCooldown:
                ActivateCooldownDecrease();
                break;

            case PowerupInventory.PowerupTypes.Multishot:
                ActivateMultishot();
                break;

            case PowerupInventory.PowerupTypes.EggReverseControls:
                ActivateEggReverseControls();
                break;

        }
    }
}
