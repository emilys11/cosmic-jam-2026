using System.Collections;
using UnityEngine;

public class EggPowerups : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] EggOnPath eggOnPath;

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
}