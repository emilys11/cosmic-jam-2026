using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class TESTAimGuide : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private Vector3 aimDirection;

    private float aimInput;
    [SerializeField] private float aimSpeed = 90f;
    [SerializeField] private float aimDistance = 10f;
    private bool useMouseAim = true;
    

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        mainCamera = Camera.main;

        aimDirection = transform.forward;
        aimDirection.y = 0f;
        aimDirection.Normalize();
    }

    

    void Update()
    {
        if (mainCamera == null) return;

        if (useMouseAim && Mouse.current != null)
        {
            // mouse aiming
            Ray ray = mainCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            Plane groundPlane = new Plane(Vector3.up, transform.position);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 target = ray.GetPoint(distance);
                target.y = transform.position.y;

                Vector3 direction = target - transform.position;

                if (direction.sqrMagnitude > 0.001f)
                {
                    aimDirection = direction.normalized;
                }
            }
        }
        else if (aimInput != 0f)
        {
            // keyboard/controller aiming
            aimDirection = Quaternion.AngleAxis(
                aimInput * aimSpeed * Time.deltaTime,
                Vector3.up
            ) * aimDirection;

            aimDirection.y = 0f;

            if (aimDirection.sqrMagnitude > 0.001f)
                aimDirection.Normalize();
        }

    
        Vector3 start = transform.position;
        Vector3 end = start + aimDirection * aimDistance;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }



    public Vector3 TESTAimDirection()
    {
        return aimDirection;
        
        //Vector3 start = lineRenderer.GetPosition(0);
        //Vector3 end = lineRenderer.GetPosition(1);

        //return (end - start).normalized;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aimInput = context.ReadValue<float>();
            useMouseAim = false;
        }
        else if (context.canceled)
        {
            aimInput = 0f;
        }
    }

}
