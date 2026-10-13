using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class TESTAimGuide : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private Vector3 aimDirection;

    [SerializeField] private float aimDistance = 10f;
    [SerializeField] private Transform model;
    [SerializeField] private float turnSpeed = 720f; // chicken turning degrees per second

    [Header("Stick Aiming")]
    [SerializeField] private float stickDeadzone = 0.25f;
    [Tooltip("Extra rotation (degrees) applied to the stick direction if the aim feels off.")]
    [SerializeField] private float stickAngleOffset = 0f;

    private Vector2 stickInput;
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

        // Moving the mouse switches back to mouse aiming
        if (!useMouseAim && Mouse.current != null &&
            Mouse.current.delta.ReadValue().sqrMagnitude > 1f)
        {
            useMouseAim = true;
        }

        if (useMouseAim && Mouse.current != null)
        {
            UpdateMouseAim();
        }
        else if (stickInput.magnitude > stickDeadzone)
        {
            UpdateStickAim();
        }
        // if the stick is released, the aim stays where it was

        Vector3 start = transform.position;
        Vector3 end = start + aimDirection * aimDistance;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // rotate model
        if (model != null)
        {
            Quaternion targetRot = Quaternion.LookRotation(aimDirection, Vector3.up);
            model.rotation = Quaternion.RotateTowards(
                model.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
    }

    private void UpdateMouseAim()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            target.y = transform.position.y;

            Vector3 direction = target - transform.position;

            if (direction.sqrMagnitude > 0.001f)
                aimDirection = direction.normalized;
        }
    }

    private void UpdateStickAim()
    {
        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0f;

        if (camForward.sqrMagnitude < 0.001f)
        {
            camForward = mainCamera.transform.up;
            camForward.y = 0f;
        }
        camForward.Normalize();

        Vector3 camRight = mainCamera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 direction = camRight * stickInput.x + camForward * stickInput.y;
        direction = Quaternion.AngleAxis(stickAngleOffset, Vector3.up) * direction;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
            aimDirection = direction.normalized;
    }

    public Vector3 TESTAimDirection()
    {
        return aimDirection;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            stickInput = context.ReadValue<Vector2>();

            if (stickInput.magnitude > stickDeadzone)
                useMouseAim = false;
        }
        else if (context.canceled)
        {
            stickInput = Vector2.zero;
        }
    }
}