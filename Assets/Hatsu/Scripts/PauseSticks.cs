using UnityEngine;

public class PauseSticks : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] float animDuration;
    private float positionSpeed;
    public bool animate;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        transform.position = startPos.transform.position;
        startPos.transform.rotation = transform.rotation;

        positionSpeed = Vector3.Distance(endPos.transform.position, startPos.transform.position) / animDuration;
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    void Update()
    {
        if (animate)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos.transform.position, positionSpeed * Time.unscaledDeltaTime);
            // transform.rotation = Quaternion.RotateTowards(startPos.transform.rotation, endPos.transform.rotation, 1f * Time.deltaTime);
        }
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 1f);
    }
    public void Reset()
    {
        animate = false;
        transform.position = startPos.transform.position;
        transform.rotation = startPos.transform.rotation;
    }
    public void ScaleStick(float scaleMultiplier)
    {
        targetScale = originalScale * scaleMultiplier;
    }    
    public void startAnimation()
    {
        animate = true;   
    }



}
