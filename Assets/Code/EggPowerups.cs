using System.Collections;
using UnityEngine;

public class EggPowerups : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] EggOnPath eggOnPath; // MUST ASSIGN.

    [Header("Power Parameters")]
    [SerializeField] public float powerupTimer = 1f;

    [Header("Speed Parameters")]
    [SerializeField] public float speedMultiplier = 2f;

    [Header("Disappearance Parameters")]
    [SerializeField] private float dissolveDuration = 1.5f;

    [ContextMenu("Enable Secret Spline")]
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
    private void DissolveEgg()
    {
        if (eggOnPath != null)
        {
            StartCoroutine(eggOnPath.DissolveCycleRoutine(eggOnPath.gameObject, powerupTimer, dissolveDuration, true));
        }
    }

    [ContextMenu("Multiply Speed")]
    public void TestMultiplySpeed()
    {
        StartCoroutine(MultiplySpeedRoutine());
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
}