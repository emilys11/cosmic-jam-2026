using UnityEngine;
using UnityEngine.EventSystems;

public class TitleRingScript : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    private Vector3 originalScale;
    private Vector3 targetScale;
    
    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    void LateUpdate()
    {
        transform.Rotate(new Vector3(0,0,1), spinSpeed * Time.deltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 1f);
    }
    
    public void ScaleRing(float scaleMultiplier)
    {
        targetScale = originalScale * scaleMultiplier;
    }
}
