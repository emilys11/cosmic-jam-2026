using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EggPowerups : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] EggOnPath eggOnPath; // MUST ASSIGN.
    [SerializeField] PowerupInventory inventory; // MUST ASSIGN THIS TOO.

    [Header("Power Parameters")]
    [SerializeField] public float powerupTimer = 1f;

    [Header("Speed Parameters")]
    [SerializeField] public float speedMultiplier = 2f;

    [Header("Disappearance Parameters")]
    [SerializeField] private float dissolveDuration = 1.5f;

    [ContextMenu("Enable Secret Spline")]

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("egg touched: " + other.gameObject.name);

        PowerupOrb orb = other.GetComponent<PowerupOrb>();

        if (orb == null) return;

        // can only collect egg type orbs
        if (orb.GetOrbType() != PowerupOrb.OrbType.Egg) return;

        CollectEggOrb();

        Destroy(other.gameObject);
    }
    private void CollectEggOrb()
    {
        PowerupInventory.PowerupTypes[] eggPowerups =
        {
            PowerupInventory.PowerupTypes.SpeedUpEgg,
            PowerupInventory.PowerupTypes.WormHole,
            PowerupInventory.PowerupTypes.SummonDecoy,
            PowerupInventory.PowerupTypes.ExpandOrbital,
            PowerupInventory.PowerupTypes.GoInvisible
        };

        PowerupInventory.PowerupTypes randomPowerup = eggPowerups[Random.Range(0, eggPowerups.Length)];
        inventory.AddPowerup(randomPowerup);

        //Debug.Log("Egg collected powerup: " + randomPowerup);
    }

    private void UndissolveOrbital()
    {
        if (eggOnPath != null && eggOnPath.secretSpline != null)
        {
            eggOnPath.SetActiveSpline();
            StartCoroutine(eggOnPath.DissolveCycleRoutine(eggOnPath.secretSpline.gameObject, powerupTimer * 999f, dissolveDuration, false));
        }
    }

    IEnumerator MultiplySpeedRoutine()
    {
        if (eggOnPath == null) yield break;

        eggOnPath.maxMoveSpeed *= speedMultiplier;
        yield return new WaitForSeconds(powerupTimer);
        eggOnPath.maxMoveSpeed /= speedMultiplier; 
    }

    [ContextMenu("Trigger Dissolve")]
    private void TriggerDissolve()
    {
        if (eggOnPath != null)
        {
            StartCoroutine(eggOnPath.DissolveCycleRoutine(eggOnPath.meshObject, powerupTimer, dissolveDuration, true));
        }
    }

    [ContextMenu("Multiply Speed")]
    public void MultiplySpeed()
    {
        StartCoroutine(MultiplySpeedRoutine());
    }

    [ContextMenu("Trigger Wormhole")]
    public void TriggerWormHole()
    {
        StartCoroutine(CallWormhole());
    }

    private IEnumerator CallWormhole()
    {
        StartCoroutine(eggOnPath.DissolveCycleRoutine(eggOnPath.meshObject, 0.1f, dissolveDuration, true));
        yield return new WaitForSeconds(dissolveDuration);
        eggOnPath.WormHole();
    }

    [ContextMenu("Spawn Decoy")]
    public void SpawnDecoy()
    {
        if (eggOnPath == null) return;

        // Instantiate a clone of the player directly using the existing reference
        GameObject decoyObj = Instantiate(eggOnPath.gameObject, eggOnPath.transform.position, eggOnPath.transform.rotation);

        // Grab the cloned EggOnPath component
        EggOnPath decoyController = decoyObj.GetComponent<EggOnPath>();
        if (decoyController != null)
        {
            // Invert its movement direction so it travels opposite to you
            decoyController.isClockwise = !eggOnPath.isClockwise;

            // Clean up the decoy after the powerup timer expires
            StartCoroutine(DecoyBrain(decoyObj, powerupTimer));
        }
    }

    private IEnumerator DecoyBrain(GameObject decoy, float delay)
    {
    float elapsed = 0f;
        while (elapsed < delay)
        {
            // Wait a random amount of time before trying to switch tracks
            float interval = Random.Range(1f, 2.5f);
            yield return new WaitForSeconds(interval);
            elapsed += interval;

            if (decoy != null)
            {
                decoy.GetComponent<EggOnPath>().SwitchToRandomSpline();
            }
            else
            {
                break;
            }
        }
        yield return new WaitForSeconds(delay);
        if (decoy != null)
        {
            if (eggOnPath != null)
            {
                StartCoroutine(decoy.GetComponent<EggOnPath>().DissolveCycleRoutine(decoy.gameObject, powerupTimer, dissolveDuration, true));
            }
            yield return new WaitForSeconds(powerupTimer + dissolveDuration);
            Destroy(decoy);
        }
    }
    public void UsePowerups(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PowerupInventory.PowerupTypes type = inventory.UseInventoryPowerup();

            switch(type)
            {
                case PowerupInventory.PowerupTypes.SpeedUpEgg:
                    MultiplySpeed();
                    break;

                case PowerupInventory.PowerupTypes.WormHole:
                    TriggerWormHole();
                    break;

                case PowerupInventory.PowerupTypes.SummonDecoy:
                    SpawnDecoy();
                    break;

                case PowerupInventory.PowerupTypes.ExpandOrbital:
                    UndissolveOrbital();
                    break;
            
                case PowerupInventory.PowerupTypes.GoInvisible:
                    TriggerDissolve();
                    break;
            }
        }
        
    }
}