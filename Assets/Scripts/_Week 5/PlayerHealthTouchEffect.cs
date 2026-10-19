using UnityEngine;

public class PlayerHealthTouchEffect : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Health Change")]
    public int healthChange = 1;

    // Positive number = heal.
    // Negative number = damage.
    // Example: 5 heals, -5 damages.

    [Header("Behaviour")]
    public bool destroyAfterTouch = true;

    [Header("Feedback")]
    public AudioSource audioSource;
    public AudioClip touchSound;
    public GameObject touchParticlePrefab;
    public float particleLifetime = 2f;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Check whether the object that entered the trigger
        // has the Player tag.
        // If it does not, stop this function.
        if (!other.CompareTag("Player"))
        {
            return;
        }



        // TODO: Get the PlayerHealth component from the object
        // that entered the trigger.
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();



        // TODO: Check whether playerHealth is null.
        // If it is, stop this function.
        if (playerHealth == null)
        {
            return;
        }

        // TODO: Check whether healthChange is greater than 0.
        // If it is:
        // - Heal the player by healthChange.
        // - Print the amount healed if debug logs are enabled.
        if (healthChange > 0)
        {
            playerHealth.Heal(healthChange);
            if (showDebugLogs)
            {
                Debug.Log("PlayerHealthTouchEffect: Healed Player for " + healthChange);
            }
        }

        // TODO: Otherwise, check whether healthChange is less than 0.
        // If it is:
        // - Damage the player using the positive version of healthChange.
        // - Print the amount of damage if debug logs are enabled.
        else if (healthChange < 0)
        {
            playerHealth.TakeDamage(-healthChange);
            if (showDebugLogs)
            {
                Debug.Log("PlayerHealthTouchEffect: Damaged Player for " + -healthChange);
            }
        }

        // TODO: Otherwise, healthChange must be 0.
        // If debug logs are enabled, print that no effect occurred.
        else
        {
            if (showDebugLogs)
            {
                Debug.Log("PlayerHealthTouchEffect: No effect occurred because healthChange is 0.");
            }
        }

        // TODO: Call the function that plays the feedback.
        PlayFeedback();

        // TODO: Check whether destroyAfterTouch is true.
        if (destroyAfterTouch == true)
        {
            // Remove this GameObject after it has been used.
            Destroy(gameObject);
        }
    }

    private void PlayFeedback()
    {
        // This uses Instantiate and Destroy.
        // These will be covered later in the trimester.
        if (touchParticlePrefab != null)
        {
            GameObject particleObject = Instantiate(touchParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particleObject, particleLifetime);
        }

        // TODO: Check that audioSource and touchSound are not null.
        // If they exist, play touchSound using the AudioSource.
        if (audioSource != null && touchSound != null)
        {
            audioSource.PlayOneShot(touchSound);
        }

    }
}