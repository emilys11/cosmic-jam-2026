
using UnityEngine;
using System;

public class EggHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    // Hassan's egg audio setup
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] collisionClips;
    [SerializeField] AudioClip[] deathClips;

    private int currentHealth;
    private bool defeated;

    public event Action OnEggDied;
    public bool isDefeated()
    {
        return defeated;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void playDamageSound()
    {
        if (audioSource == null || collisionClips == null || collisionClips.Length == 0)
        {
            return;
        }

        audioSource.PlayOneShot(collisionClips[UnityEngine.Random.Range(0, collisionClips.Length)]);
    }

    float playDeathSound()
    {
        if (audioSource == null || deathClips == null || deathClips.Length == 0)
        {
            return 0f;
        }

        AudioClip selectedClip = deathClips[UnityEngine.Random.Range(0, deathClips.Length)];
        audioSource.PlayOneShot(selectedClip);
        return selectedClip.length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (defeated || !other.TryGetComponent(out Seed seed))
            return;

        if (GameManager.Instance == null ||
            !GameManager.Instance.IsPlaying)
            return;

        TakeDamage();
        playDamageSound();
        Destroy(seed.gameObject);
    }

    private void TakeDamage()
    {
        currentHealth--;

        Debug.Log("Egg HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            defeated = true;
            playDeathSound();
            OnEggDied?.Invoke();
        }
    }
}
