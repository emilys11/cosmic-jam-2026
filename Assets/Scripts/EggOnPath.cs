using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.InputSystem; // Required for the new Input System

public class BeadOnPath : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The SplineContainer GameObject the bead will follow.")]
    public SplineContainer targetSpline;

    [Header("Movement")]
    [Range(0f, 1f)]
    [Tooltip("Progress along the path (0 = start, 1 = end).")]
    public float progress = 0f;

    [Tooltip("Speed at which the bead moves along the path.")]
    public float moveSpeed = 0.5f;

    [Tooltip("Should the bead loop back around when it hits the ends?")]
    public bool isLooping = true;

    void Update()
    {
        if (targetSpline == null || Keyboard.current == null) return;

        // Read input using the new Input System keyboard API
        float inputAxis = 0f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            inputAxis += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            inputAxis -= 1f;

        if (Mathf.Abs(inputAxis) > 0.01f)
        {
            float length = targetSpline.Spline.GetLength();
            if (length > 0f)
            {
                progress += (inputAxis * moveSpeed / length) * Time.deltaTime;
            }
        }

        // Handle path bounds (looping or clamping)
        if (progress > 1f)
        {
            progress = isLooping ? progress % 1f : 1f;
        }
        else if (progress < 0f)
        {
            progress = isLooping ? (1f + (progress % 1f)) : 0f;
        }

        // Evaluate position and direction along the spline
        Vector3 position = targetSpline.EvaluatePosition(progress);
        float3 tangent = targetSpline.EvaluateTangent(progress);

        // Apply position
        transform.position = position;

        // Optionally align rotation with the tube's direction
        if (math.lengthsq(tangent) > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Normalize(tangent));
        }
    }
}