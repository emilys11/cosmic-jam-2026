
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class AimGuide : MonoBehaviour
{
    [SerializeField] private SplineContainer targetSpline;

    private LineRenderer lineRenderer;
    private Camera mainCamera;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (targetSpline == null || mainCamera == null ||
            Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Find the mouse direction relative to the chicken on screen.
        Vector3 chickenScreenPosition =
            mainCamera.WorldToScreenPoint(transform.position);

        Vector2 aimDirection =
            mousePosition - (Vector2)chickenScreenPosition;

        if (aimDirection.sqrMagnitude < 0.001f)
            return;

        aimDirection.Normalize();

        // Find the point on the spline that best matches
        // the mouse's direction as seen by the camera.
        float bestDot = float.NegativeInfinity;
        Vector3 targetPosition = transform.position;

        const int samples = 128;

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            Vector3 splinePosition =
                targetSpline.transform.TransformPoint(
                    (Vector3)targetSpline.Spline.EvaluatePosition(t));

            // Project each spline point onto the screen.
            Vector3 splineScreenPosition =
                mainCamera.WorldToScreenPoint(splinePosition);

            if (splineScreenPosition.z <= 0f)
                continue;

            Vector2 splineDirection =
                (Vector2)splineScreenPosition -
                (Vector2)chickenScreenPosition;

            if (splineDirection.sqrMagnitude < 0.001f)
                continue;

            splineDirection.Normalize();

            float dot = Vector2.Dot(
                aimDirection, splineDirection);

            if (dot > bestDot)
            {
                bestDot = dot;
                targetPosition = splinePosition;
            }
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }
}
