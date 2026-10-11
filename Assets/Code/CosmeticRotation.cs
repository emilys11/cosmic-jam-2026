using UnityEngine;

public class CosmeticRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Degrees per second around the X, Y, and Z axes.")]
    public Vector3 rotationSpeed = new Vector3(0f, 20f, 0f);

    void Update()
    {
        // Rotates the object smoothly every frame relative to time
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}