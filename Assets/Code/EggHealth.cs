
using UnityEngine;

public class EggHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private bool defeated;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (defeated || !other.TryGetComponent(out Seed seed))
            return;

        if (GameManager.Instance == null ||
            !GameManager.Instance.IsPlaying)
            return;

        TakeDamage();
        Destroy(seed.gameObject);
    }

    private void TakeDamage()
    {
        currentHealth--;

        Debug.Log("Egg HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            defeated = true;
            GameManager.Instance.ChickenWins();
        }
    }
}
