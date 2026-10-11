using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] float seedVelocity = 5f;
    [SerializeField] float seedLifetime = 5f; // only for testing
    [SerializeField] float ringMargin = 0.05f;
    private bool launched = false;

    private Rigidbody rb;

    private Transform center; // center of ring
    private float targetRadius; // radius of the target ring

    private ChickenPowerups chickenPowerups;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    float GetVisualRadius()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return Mathf.Max(bounds.extents.x, bounds.extents.z);
    }

    public void LaunchSeed(Vector3 direction, Transform chicken, float ringRadius, float velocityMultiplier)
    {
        // get center of the ring where chicken is, launch seed from that point
        center = chicken;
        targetRadius = Mathf.Max(0f, ringRadius - GetVisualRadius() - ringMargin);

        chickenPowerups = chicken.GetComponentInParent<ChickenPowerups>();

        direction.y = 0f;
        Quaternion facingDirection = Quaternion.LookRotation(direction.normalized, Vector3.up);

        float roll = Random.Range(0f, 360f);

        transform.rotation = facingDirection * Quaternion.Euler(0f, 0f, roll);

        rb.linearVelocity = direction.normalized * seedVelocity * velocityMultiplier;

        launched = true;

        Destroy(gameObject, seedLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        PowerupOrb orb = collision.gameObject.GetComponent<PowerupOrb>();

        // no orb or type doesnt match up
        if (orb == null) return;
        if (orb.GetOrbType() == PowerupOrb.OrbType.Chicken)
        {
            //ChickenPowerups chickenPowerups = FindAnyObjectByType<ChickenPowerups>();
            chickenPowerups.CollectOrb();

            // destroy orb and seed
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    // collision with ring -> destroy once TRAVELLED target radius
    void Update()
    {
        if (!launched || center == null) return;

        Vector3 seedPosition = transform.position;
        Vector3 centerPosition = center.position;
        seedPosition.y = 0f;
        centerPosition.y = 0f;

        float travelDistance = Vector3.Distance(seedPosition, centerPosition);
        //Debug.Log("seed distance = " + travelDistance + " // target = " + targetRadius);

        if (travelDistance >= targetRadius)
        {
            //Debug.Log("seed reached target radius");
            Destroy(gameObject); // destroy seed
        }
    }
}
