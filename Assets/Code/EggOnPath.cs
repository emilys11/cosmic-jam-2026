using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class EggOnPath : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The SplineContainers the egg can switch between.")]
    public List<SplineContainer> targetSplines = new List<SplineContainer>();
    public SplineContainer secretSpline;

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

    void Update()
    {
        if (targetSplines == null || targetSplines.Count == 0 || Keyboard.current == null) return;

        HandleSplineSwitching();
        UpdateMovementAndProgress();
        ApplyPositionAndRotation();
    }

    private void HandleSplineSwitching()
    {
        int newIndex = index;
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            newIndex = (index + 1) % targetSplines.Count;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            newIndex = (index - 1 + targetSplines.Count) % targetSplines.Count;
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
    }

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

    public IEnumerator DissolveCycleRoutine(GameObject targetObject, float disappearTime, float dissolveDuration, bool toDissolve)
    {
        if (targetObject == null) yield break;

        Renderer targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer == null) yield break;
        
        Material targetMaterial = targetRenderer.material;
        
        float startCutoff;
        float intermediateCutoff;

        // Define the first transition based on bool flag
        if (toDissolve)
        {
            // Start visible (10), go to hidden (-5)
            startCutoff = 10f;
            intermediateCutoff = -5f;
        }
        else
        {
            // Start hidden (-5), go to visible (10)
            startCutoff = -5f;
            intermediateCutoff = 10f;
        }

        // Apply starting state immediately
        targetMaterial.SetFloat("_CutoffHeight", startCutoff);
        float elapsed = 0f;

        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(startCutoff, intermediateCutoff, elapsed / dissolveDuration);
            targetMaterial.SetFloat("_CutoffHeight", newCutoff);
            yield return null;
        }
        targetMaterial.SetFloat("_CutoffHeight", intermediateCutoff);
        
        // Pause at the intermediate state
        yield return new WaitForSeconds(disappearTime);

        // The new start is where we just left off, and the final target is the opposite extreme
        float finalTarget = startCutoff; 
        elapsed = 0f;

        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float newCutoff = Mathf.Lerp(intermediateCutoff, finalTarget, elapsed / dissolveDuration);
            targetMaterial.SetFloat("_CutoffHeight", newCutoff);
            yield return null;
        }
        targetMaterial.SetFloat("_CutoffHeight", finalTarget);
    }

    public void SpeedModifier(float speedMultiplier)
    {
        maxMoveSpeed *= speedMultiplier;
    }

    public void SetActiveSpline()
    {
        secretSpline.gameObject.SetActive(true);
        targetSplines.Add(secretSpline);
    }

    public void OnClockwise(InputAction.CallbackContext context)
    {
        isClockwise = true;
    }

    public void OnCounterClockwise(InputAction.CallbackContext context)
    {
        isClockwise = false;
    }
}
