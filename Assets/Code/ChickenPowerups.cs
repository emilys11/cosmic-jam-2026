using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChickenPowerups : MonoBehaviour
{
    [SerializeField] ChickenShooting chickenShooting;
    [SerializeField] float velocityMultiplier = 5f; // TO BE TRIALED

    [SerializeField] private float powerupTimer = 15f;
    private Coroutine activePowerupCoroutine;

    [SerializeField] PowerupInventory inventory;
    [SerializeField] int requiredOrbs = 3;
    private int collectedOrbs = 0;

    private void ClearActivePowerup()
    {
        chickenShooting.DeactivateSeedVelocityIncrease();
        chickenShooting.DeactivateMultishot();
        chickenShooting.DeactivateEggReverseControls();
    }

    public void ActivateSeedVelocityIncrease()
    {
        if (activePowerupCoroutine != null)
        {
            StopCoroutine(activePowerupCoroutine);
        }

        ClearActivePowerup();

        activePowerupCoroutine = StartCoroutine(IncreaseSeedVelocity());
    }

    IEnumerator IncreaseSeedVelocity()
    {
        Debug.Log("velocity increase activated");
        chickenShooting.ActivateSeedVelocityIncrease(velocityMultiplier);
        yield return new WaitForSeconds(powerupTimer);
        Debug.Log("velocity increase deactivated");
        chickenShooting.DeactivateSeedVelocityIncrease();
        activePowerupCoroutine = null;
    }

    public void ActivateMultishot()
    {
        if (activePowerupCoroutine != null)
        {
            StopCoroutine (activePowerupCoroutine);
        }

        ClearActivePowerup();

        activePowerupCoroutine = StartCoroutine(MultishotTimer());
    }

    IEnumerator MultishotTimer()
    {
        Debug.Log("multishot activated");
        chickenShooting.ActivateMultishot();
        yield return new WaitForSeconds(powerupTimer);
        Debug.Log("multishot timed out");
        chickenShooting.DeactivateMultishot();
        activePowerupCoroutine = null;
    }

    public void ActivateEggReverseControls()
    {
        // egg controller reverse controls
        if (activePowerupCoroutine != null)
        {
            StopCoroutine(activePowerupCoroutine);
        }

        ClearActivePowerup();

        activePowerupCoroutine = StartCoroutine(EggReverseControlTimer());
    }
    IEnumerator EggReverseControlTimer()
    {
        Debug.Log("multishot activated");
        chickenShooting.ActivateEggReverseControls();
        yield return new WaitForSeconds(powerupTimer);
        Debug.Log("multishot timed out");
        chickenShooting.DeactivateEggReverseControls();
        activePowerupCoroutine = null;
    }

    // collect orbs to get random powerup
    // can reroll powerups
    public void CollectOrb()
    {
        collectedOrbs++;
        if (collectedOrbs >= requiredOrbs)
        {
            AssignRandomPowerup();
            collectedOrbs = 0;
        }
    }


    public void AssignRandomPowerup()
    {
        PowerupInventory.PowerupTypes[] chickenPowerups =
        {
            PowerupInventory.PowerupTypes.IncreaseSeedVelocity,
            PowerupInventory.PowerupTypes.Multishot,
            PowerupInventory.PowerupTypes.EggReverseControls
        };

        int randomIndex = Random.Range(0, chickenPowerups.Length);

        PowerupInventory.PowerupTypes selectedPowerup = chickenPowerups[randomIndex];

        inventory.AddPowerup(selectedPowerup);

        Debug.Log("chicken powerup: " + selectedPowerup);

    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    PowerupOrb orb = collision.gameObject.GetComponent<PowerupOrb>();
        
    //    // no orb or type doesnt match up
    //    if (orb == null) return;
    //    if (orb.GetOrbType() == PowerupOrb.OrbType.Chicken) 
    //    { 
    //        //ChickenPowerups chickenPowerups = FindAnyObjectByType<ChickenPowerups>();
    //        chickenPowerups.CollectOrb();

    //        // destroy orb and seed
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
    public void UsePowerups(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("chicken powerup INPUT WORKS");
            PowerupInventory.PowerupTypes type = inventory.UseInventoryPowerup();

            switch(type)
            {
                case PowerupInventory.PowerupTypes.IncreaseSeedVelocity:
                    ActivateSeedVelocityIncrease();
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
}
