using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class TESTAimGuide : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private Vector3 aimDirection;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null || Mouse.current == null) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            //Vector3 mouseWorldPosition = ray.GetPoint(distance);
            Vector3 target = ray.GetPoint(distance);
            target.y = transform.position.y;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target);
        }
    }

    public Vector3 TESTAimDirection()
    {
        return aimDirection;
        
        //Vector3 start = lineRenderer.GetPosition(0);
        //Vector3 end = lineRenderer.GetPosition(1);

        //return (end - start).normalized;
    }

}
