using UnityEngine;
using UnityEngine.InputSystem;

public class ChickenShooting : MonoBehaviour
{
    public GameObject seedsPrefab;
    [SerializeField] float seedCooldown = 5f;
    private float cooldownTimer = 0f;

    // select orbital ring -> shoot seed -> destroy when chicken seed hits the ring collider

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    public void onFire(InputValue value)
    {
        if (value.isPressed)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (cooldownTimer > 0f)
        {
            return;
        }

        GameObject activeSeed = Instantiate(seedsPrefab, transform.position, transform.rotation);

        cooldownTimer = seedCooldown;

    }
}
