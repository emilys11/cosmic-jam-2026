using System.Collections;
using UnityEngine;

public class ChickenPowerups : MonoBehaviour
{
    [SerializeField] ChickenShooting chickenShooting;
    [SerializeField] float cooldownDecreaseMultiplier = 0.8f; // TO BE TRIALED

    [SerializeField] float powerupTimer = 0f;
    private Coroutine decreaseCooldownCoroutine;
    private Coroutine multishotCoroutine;

    //public void DecreaseCooldown()
    //{
    //    chickenShooting.DecreaseFireCooldown(cooldownDecreaseMultiplier);
    //}

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

    //public void MultishotUpgrade()
    //{
    //    chickenShooting.Multishot();
    //}
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

    public void ReverseControlsUpgrade()
    {
        // egg controller reverse controls
    }
}
