using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using System.Collections;

public class EggOnPath : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The SplineContainers the egg can switch between.")]
    public SplineContainer[] targetSplines;

    [Header("Movement Settings")]
    [Range(0f, 1f)]
    public float progress = 0f;
    public float maxMoveSpeed = 0.5f;

    [Header("Acceleration & Deceleration")]
    public bool enableAcceleration = false;
    public float accelerationRate = 2f;
    public float decelerationRate = 4f;
    private float currentSpeed = 0f;

    [Header("Spline Switch Transition")]
    public bool enableSwitchLerp = true;
    [Tooltip("Duration in seconds to smoothly slide/arc to the new track.")]
    public float switchDuration = 0.25f;
    private bool isTransitioning = false;
    private float transitionTimer = 0f;
    private int oldIndex = 0;

    [Header("Path Options")]
    public bool isLooping = true;
    public bool isClockwise = true;

    // Index to change Splines.
    private int index = 0;

    // gameObject Materials.
    private Material objectMaterial;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
        }
    }


    void Update()
    {
        if (targetSplines == null || targetSplines.Length == 0) return;

        UpdateMovementAndProgress();
        ApplyPositionAndRotation();
    }

    /*private void HandleSplineSwitching()
    {
        int newIndex = index;
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            newIndex = (index + 1) % targetSplines.Length;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            newIndex = (index - 1 + targetSplines.Length) % targetSplines.Length;
        }

        // Trigger transition if index changed
        if (newIndex != index)
        {
            if (enableSwitchLerp && switchDuration > 0f)
            {
                oldIndex = index;
                index = newIndex;
                isTransitioning = true;
                transitionTimer = 0f;
            }
            else
            {
                index = newIndex;
            }
        }
    }*/

    private void UpdateMovementAndProgress()
    {
        // Read Input (A / D equivalent via isClockwise flag)
        float inputAxis = 0f;
        if (isClockwise)
            inputAxis += 1f;
        if (!isClockwise)
            inputAxis -= 1f;

        // Calculate Acceleration / Deceleration
        float targetSpeed = inputAxis * maxMoveSpeed;
        if (enableAcceleration)
        {
            float rate = (Mathf.Abs(inputAxis) > 0.01f) ? accelerationRate : decelerationRate;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        // Update Progress Along the Spline
        if (Mathf.Abs(currentSpeed) > 0.001f)
        {
            float length = targetSplines[index].Spline.GetLength();
            if (length > 0f)
            {
                progress += (currentSpeed / length) * Time.deltaTime;
            }
        }

        // Handle Path Bounds
        if (progress > 1f)
        {
            progress = isLooping ? progress % 1f : 1f;
        }
        else if (progress < 0f)
        {
            progress = isLooping ? (1f + (progress % 1f)) : 0f;
        }
    }

    private void ApplyPositionAndRotation()
    {
        Vector3 finalPosition;
        Quaternion finalRotation;

        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / switchDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Diagonal blend between old and new splines at current progress
            Vector3 posOld = targetSplines[oldIndex].EvaluatePosition(progress);
            Vector3 posNew = targetSplines[index].EvaluatePosition(progress);
            finalPosition = Vector3.Lerp(posOld, posNew, smoothT);

            float3 tanOld = targetSplines[oldIndex].EvaluateTangent(progress);
            float3 tanNew = targetSplines[index].EvaluateTangent(progress);
            float3 blendTan = math.lerp(tanOld, tanNew, smoothT);

            finalRotation = math.lengthsq(blendTan) > 0.001f
                ? Quaternion.LookRotation(Vector3.Normalize(blendTan))
                : transform.rotation;

            if (t >= 1f)
            {
                isTransitioning = false;
            }
        }
        else
        {
            // Standard evaluation
            finalPosition = targetSplines[index].EvaluatePosition(progress);
            float3 tangent = targetSplines[index].EvaluateTangent(progress);

            finalRotation = math.lengthsq(tangent) > 0.001f
                ? Quaternion.LookRotation(Vector3.Normalize(tangent))
                : transform.rotation;
        }

        transform.position = finalPosition;
        transform.rotation = finalRotation;
    }

    public IEnumerator DissolveRoutine(float disappearTime, float dissolveDuration, bool isDissolving)
    {
        isDissolving = true;

        float targetCutoff = -5;
        float startCutoff = objectMaterial.GetFloat("_CutoffHeight");
        float elapsed = 0f;

        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(startCutoff, targetCutoff, elapsed / dissolveDuration);
            objectMaterial.SetFloat("_CutoffHeight", newCutoff);
            yield return null;
        }
        objectMaterial.SetFloat("_CutoffHeight", targetCutoff);
        
        yield return new WaitForSeconds(disappearTime);

        startCutoff = targetCutoff;
        elapsed = 0f;
        targetCutoff = 10;
        
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(startCutoff, targetCutoff, elapsed / dissolveDuration);
            objectMaterial.SetFloat("_CutoffHeight", newCutoff);
            yield return null;
        }
        objectMaterial.SetFloat("_CutoffHeight", targetCutoff);
        isDissolving = false;
    }
    public void OnClockwise(InputAction.CallbackContext context)
    public void OnClockwiseSwitch(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            UnityEngine.Debug.Log("Clockwise switch");
            isClockwise = !isClockwise;
        }
        
    }


    public void OnRingUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            int newIndex = index;
            newIndex = (index + 1) % targetSplines.Length;
            TriggerTransition(newIndex);
        }
    }

    public void OnRingDown(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            int newIndex = index;
            newIndex = (index - 1 + targetSplines.Length) % targetSplines.Length;
            TriggerTransition(newIndex);
        }
        
    }

    void TriggerTransition(int newIndex)
    {
        // Trigger transition if index changed
        if (newIndex != index)
        {
            if (enableSwitchLerp && switchDuration > 0f)
            {
                oldIndex = index;
                index = newIndex;
                isTransitioning = true;
                transitionTimer = 0f;
            }
            else
            {
                index = newIndex;
            }
        }
    }
}
