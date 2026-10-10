using UnityEngine;
using UnityEngine.InputSystem;

public class ChickenShooting : MonoBehaviour
{
    public GameObject seedsPrefab;
    [SerializeField] float seedCooldown = 0.5f;
    private float cooldownTimer = 0f;

    // select orbital ring -> shoot seed -> destroy when chicken seed hits the ring collider
    [SerializeField] LineRenderer lineRenderer;

    // Hassan's chicken shooting audio (is this correct? :shrug:)
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] fireClips; // made 3 shooting noise variations

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        // Shoot when Space is pressed
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("space pressed");
            Fire();
        }
    }
    //public void onFire(InputValue value)
    //{
    //    Debug.Log("OnFire called");
    //    if (value.isPressed)
    //    {
    //        Fire();
    //    }
    //}

    void FireSound()
    {
        if (audioSource == null || fireClips == null || fireClips.Length == 0)
        {
            return;
        }

        audioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)]);
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

        GameObject activeSeed = Instantiate(seedsPrefab, transform.position, Quaternion.identity);
        
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
        seed.LaunchSeed(shootDirection);
        FireSound();

        cooldownTimer = seedCooldown;

    }
}
