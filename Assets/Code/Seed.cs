using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] float seedVelocity = 1f;
    [SerializeField] float seedLifetime = 5f; // only for testing

    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void LaunchSeed(Vector3 direction)
    {
        direction.y = 0f;

        rb.linearVelocity = direction.normalized * seedVelocity;

        Destroy(gameObject, seedLifetime);
    }

    // destructs on collision with rings
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Orbital Ring"))
    //    {
    //        Destroy (gameObject);
    //    }
    //}


    //private void OnTriggerEnter(Collider other)
    //{
        
    //}
}
