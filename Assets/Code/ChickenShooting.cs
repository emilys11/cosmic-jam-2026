using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class ChickenShooting : MonoBehaviour
{
    public GameObject seedsPrefab;
    [SerializeField] float seedCooldown = 0.5f;
    private float cooldownTimer = 0f;

    // select orbital ring -> shoot seed -> destroy when chicken seed hits the ring collider
    [SerializeField] LineRenderer lineRenderer;

    [Header("Orbital Rings")]
    [SerializeField] SplineContainer[] rings;
    private int selectedRingIndex = 0; // default innermost ring
    private float selectedRingRadius;

 
    void Start()
    {
        UpdateSelectedRing();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        
        // PLACEHOLDER FOR INPUT
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("left mb pressed");
            Fire();
        }
        // left = move inward
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            //Debug.Log("pressed Q");
            SelectInwardRing();
        }

        // right = move outward
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            //Debug.Log("pressed E");
            SelectOutwardRing();
        }
    }

    void Fire()
    {
        if (cooldownTimer > 0f)
        {
            return;
        }

        // get line direction
        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);
        Vector3 shootDirection = (end - start).normalized;
        //Debug.Log(
        //    "FIRING // selected radius = " + selectedRingRadius +
        //    " // direction = " + shootDirection
        //);

        GameObject activeSeed = Instantiate(seedsPrefab, transform.position, Quaternion.identity);
        //Debug.Log("SEED CREATED: " + activeSeed.name);

        // IGNORE COLLISIONS BETWEEN SEED AND CHICKEN
        Collider[] chickenColliders = GetComponentsInChildren<Collider>();
        Collider[] seedColliders = activeSeed.GetComponentsInChildren<Collider>();

        foreach (Collider chickenCollider in chickenColliders)
        {
            foreach (Collider seedCollider in seedColliders)
            {
                Physics.IgnoreCollision(chickenCollider, seedCollider);
            }
        }

        // shoot seed
        Seed seed = activeSeed.GetComponent<Seed>();
        if (seed == null)
        {
            //Debug.LogError("Seed prefab does not have Seed.cs!");
            return;
        }
        seed.LaunchSeed(shootDirection, transform, selectedRingRadius);

        cooldownTimer = seedCooldown;

    }

    void UpdateSelectedRing()
    {
        SplineContainer selectedRing = rings[selectedRingIndex];

        float ringCircumference = selectedRing.CalculateLength ();
        selectedRingRadius = ringCircumference/(2f * Mathf.PI); // c = 2*pi*r

        //Debug.Log("selected ring = " + selectedRingIndex + " // radius = " + selectedRingRadius);
    }

    void SelectInwardRing()
    {
        if (selectedRingIndex > 0)
        {
            selectedRingIndex--;
            UpdateSelectedRing();
        }
    }
    void SelectOutwardRing()
    {
        if (selectedRingIndex < rings.Length - 1)
        {
            selectedRingIndex++;
            UpdateSelectedRing();
        }
    }
}
