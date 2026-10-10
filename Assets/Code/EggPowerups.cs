using System.Collections;
using UnityEngine;

public class EggPowerups : MonoBehaviour
{
    [Header("Power Parameters")]
     [SerializeField] public float powerTime = 1f;

    [Header("Disappearance Parameters")]
    [SerializeField] private float targetCutoff = -5f;
    [SerializeField] private float dissolveDuration = 1.5f;

    private Material objectMaterial;
    private bool isDissolving;

    // This adds a button to the component's context menu (three dots icon in the Inspector)
    [ContextMenu("Trigger Dissolve")]
    public void DissolveEgg()
    {
        if (!isDissolving && objectMaterial != null)
        {
            StartCoroutine(gameObject.GetComponent<EggOnPath>().DissolveRoutine(powerTime, dissolveDuration, isDissolving));
        }
    }

}