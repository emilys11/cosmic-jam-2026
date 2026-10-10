using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class AimGuide : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, mouseWorldPosition);
        }
    }
}