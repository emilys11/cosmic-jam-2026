using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class ChickenShooting : MonoBehaviour
{
    public GameObject seedsPrefab;
    [SerializeField] float seedCooldown = 0.9f;
    private float cooldownTimer = 0f;

    // select orbital ring -> shoot seed -> destroy when chicken seed hits the ring collider
    [SerializeField] LineRenderer lineRenderer;

    [Header("Orbital Rings")]
    [SerializeField] SplineContainer[] rings;
    private int selectedRingIndex = 0; // default innermost ring
    private float selectedRingRadius;

    // POWERUPS
    [SerializeField] bool multishotActive = false;
    [SerializeField] float multishotAngle = 15f;

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
        
        // PLACEHOLDER FOR INPUT (lowk will keep this for people who prefer mouse)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("left mb pressed");
            Fire();
        }
        // left = move inward
        /*if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            //Debug.Log("pressed Q");
            SelectInwardRing();
        }

        // right = move outward
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            //Debug.Log("pressed E");
            SelectOutwardRing();
        }*/
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

        if (multishotActive)
        {
            SpawnSeed(shootDirection); // center
            Vector3 leftDirection = Quaternion.AngleAxis(-multishotAngle, Vector3.up) * shootDirection; // left
            Vector3 rightDirection = Quaternion.AngleAxis(multishotAngle, Vector3.up) * shootDirection; // right

            SpawnSeed(leftDirection);
            SpawnSeed(rightDirection);
        }
        else
        {
            SpawnSeed(shootDirection);
        }

        cooldownTimer = seedCooldown;

    }

    void SpawnSeed(Vector3 direction)
    {
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
        seed.LaunchSeed(direction, transform, selectedRingRadius);
    }

    void UpdateSelectedRing()
    {
        SplineContainer selectedRing = rings[selectedRingIndex];

        float ringCircumference = selectedRing.CalculateLength ();
        selectedRingRadius = ringCircumference/(2f * Mathf.PI); // c = 2*pi*r
    }

    public void SelectInwardRing(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (selectedRingIndex > 0)
            {
                selectedRingIndex--;
                UpdateSelectedRing();
            }
        }
        
    }
    public void SelectOutwardRing(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (selectedRingIndex < rings.Length - 1)
            {
                selectedRingIndex++;
                UpdateSelectedRing();
            }
        }
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire();
        }
    }

    // POWER UP FUNCTIONS
    public void IncreaseFireRate(float multiplier)
    {
        seedCooldown *= multiplier;
        seedCooldown = Mathf.Max(seedCooldown, 0.1f);
    }
    public void Multishot()
    {
        multishotActive = true;

        Debug.Log("Multishot active: 1 -> 3 seeds");
    }
}
