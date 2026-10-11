//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Splines;

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

    private SplineContainer targetSpline;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    float GetVisualRadius()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return 0f;
        return Mathf.Max(col.bounds.extents.x, col.bounds.extents.z);
    }

    public void LaunchSeed(Vector3 direction, Transform chicken, float ringRadius, float velocityMultiplier, SplineContainer selectedSpline)
    {
        // get center of the ring where chicken is, launch seed from that point
        center = chicken;
        targetRadius = Mathf.Max(0f, ringRadius - GetVisualRadius() - ringMargin);

        chickenPowerups = chicken.GetComponentInParent<ChickenPowerups>();

        direction.y = 0f;
        Quaternion facingDirection = Quaternion.LookRotation(direction.normalized, Vector3.up);

        float roll = Random.Range(0f, 360f);

        transform.rotation = facingDirection * Quaternion.Euler(0f, 0f, roll);
        
        // DO NOT REMOVE THIS PLEASE!!!!!! THIS MAKES THE SEED SPAWN AND MOVE :SOB: :WILTED ROSE:
        float finalSpeed = seedVelocity * velocityMultiplier;
        rb.linearVelocity = direction * finalSpeed;

        launched = true;

        targetSpline = selectedSpline; //For communicating with egg selected spline

        Destroy(gameObject, seedLifetime);
    }

    public SplineContainer getTargetSpline()
    {
        return targetSpline;
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
            orb.SpawnOrbPickupVFX();
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
