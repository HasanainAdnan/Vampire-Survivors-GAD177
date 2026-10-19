using UnityEngine;

public class EnemyDamageOnTouch : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Damage")]
    public int contactDamage = 1;

    [Header("Knockback")]
    public bool applyKnockback = true;
    public float knockbackDistance = 0.5f;

    [Header("Audio Feedback")]
    public AudioSource audioSource;
    public AudioClip hitSound;

    [Range(0f, 1f)]
    public float hitSoundVolume = 1f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private void Start()
    {
        // TODO: Check if audioSource is null.
        // If it is, get the AudioSource component from this GameObject.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Check whether the object involved in the collision
        // has the Player tag.
        // If it does not, stop this function.
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }



        // TODO: Get the PlayerHealth component from the
        // object involved in the collision.
        PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();



        // TODO: Check that playerHealth is not null.
        // If it exists:
        // - Damage the player using contactDamage.
        // - Call PlayHitSound.
        // - Print the damage dealt if debug logs are enabled.
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
            PlayHitSound();

            if (showDebugLogs == true)
            {
                Debug.Log("EnemyDamageOnTouch: Damaged Player for " + contactDamage);
            }
        }

        // TODO: Check whether knockback is enabled.
        if (applyKnockback == true)
        {
            // TODO: Get the Rigidbody2D attached to the player's collider.
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();

            // TODO: Check that playerRb is not null.
            if (playerRb != null)
            {
                // Calculate the direction from the enemy to the player.
                Vector2 direction = (playerRb.position - (Vector2)transform.position).normalized;

                // Calculate where the Rigidbody2D should move to.
                Vector2 targetPosition = playerRb.position + direction * knockbackDistance;

                // TODO: Move the player's Rigidbody2D to targetPosition. Hint you'll need to use the Move Function of rigidbody.
                playerRb.MovePosition(targetPosition);

                // TODO: If debug logs are enabled,
                // print the new knockback position.
                if (showDebugLogs == true)
                {
                    Debug.Log("EnemyDamageOnTouch: Knockback Player to " + targetPosition);
                }
            }
        }
    }

    private void PlayHitSound()
    {
        // TODO: Check whether audioSource is null.
        // If it is, stop this function.
        if (audioSource == null)
        {
            return;
        }

        // TODO: Check whether hitSound is null.
        // If it is, stop this function.
        if (hitSound == null)
        {
            return;
        }

        // TODO: Play hitSound using the AudioSource
        // and the hitSoundVolume.
        audioSource.PlayOneShot(hitSound, hitSoundVolume);

        // TODO: If debug logs are enabled,
        // print that the hit sound played.
        if (showDebugLogs == true)
        {
            Debug.Log("EnemyDamageOnTouch: Played Hit Sound");
        }


    }
}