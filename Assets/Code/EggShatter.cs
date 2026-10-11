using UnityEngine;

public class EggShatter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EggHealth eggHealth;
    [SerializeField] private GameObject intactEgg;
    [SerializeField] private GameObject fracturedEgg;
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private ParticleSystem burstEffect; // optional

    [Header("Explosion")]
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float upwardsModifier = 0.5f;

    private Rigidbody[] pieces;

    private void Awake()
    {
        pieces = fracturedEgg.GetComponentsInChildren<Rigidbody>(true);
        fracturedEgg.SetActive(false);
    }

    private void OnEnable()
    {
        if (eggHealth != null)
            eggHealth.OnEggDied += Shatter;
    }

    private void OnDisable()
    {
        if (eggHealth != null)
            eggHealth.OnEggDied -= Shatter;
    }

    private void Shatter()
    {
        intactEgg.SetActive(false);
        fracturedEgg.SetActive(true);

        foreach (Rigidbody rb in pieces)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, explosionPoint.position,
                explosionRadius, upwardsModifier, ForceMode.Impulse);
        }

        if (burstEffect != null)
            burstEffect.Play();
    }
}
