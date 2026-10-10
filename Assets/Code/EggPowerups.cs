using System.Collections;
using UnityEngine;

public class EggPowerups : MonoBehaviour
{
    [Header("Disappearance Parameters")]
    [SerializeField] private string cutoffPropertyName = "_CutoffHeight";
    [SerializeField] private float targetCutoff = -5f;
    [SerializeField] private float dissolveDuration = 1.5f;
    [SerializeField] public float DisappearTime = 1f;

    private Material objectMaterial;
    private bool isDissolving = false;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
        }
    }

    // This adds a button to the component's context menu (three dots icon in the Inspector)
    [ContextMenu("Trigger Dissolve")]
    public void DissolveEgg()
    {
        if (!isDissolving && objectMaterial != null)
        {
            StartCoroutine(DissolveRoutine());
        }
    }

    private IEnumerator DissolveRoutine()
    {
        isDissolving = true;
        
        float startCutoff = objectMaterial.GetFloat(cutoffPropertyName);
        float elapsed = 0f;

        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(startCutoff, targetCutoff, elapsed / dissolveDuration);
            objectMaterial.SetFloat(cutoffPropertyName, newCutoff);
            yield return null;
        }
        objectMaterial.SetFloat(cutoffPropertyName, targetCutoff);
        
        yield return new WaitForSeconds(DisappearTime);

        startCutoff = targetCutoff;
        elapsed = 0f;
        targetCutoff = 10;
        
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(startCutoff, targetCutoff, elapsed / dissolveDuration);
            objectMaterial.SetFloat(cutoffPropertyName, newCutoff);
            yield return null;
        }
        objectMaterial.SetFloat(cutoffPropertyName, targetCutoff);
    }
}