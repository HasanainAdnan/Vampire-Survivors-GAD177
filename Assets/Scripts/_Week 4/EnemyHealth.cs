using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Health")]
    public int maxHP = 3;
    public int currentHP;

    [Header("Rewards")]
    public int xpReward = 5;
    public int moneyReward = 1;
    public int killReward = 1;

    [Header("Reward References")]
    public PlayerExperience playerExperience;
    public PlayerMoney playerMoney;
    public PlayerKills playerKills;

    [Header("Coin Drops")]
    public GameObject coinPrefab;
    public int coinsToDrop = 1;

    [Range(0f, 1f)]
    public float coinDropChance = 0.5f;

    public float coinSpreadRadius = 0.5f;

    [Header("Hit Flash")]
    public SpriteRenderer spriteRenderer;
    public bool useHitFlash = true;
    public Color hitFlashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    [Header("Death Event")]
    public UnityEvent onDie;

    [Header("Debug")]
    public bool enableDebugKeys = true;
    public bool showDebugLogs = true;
    public Key debugDamageKey = Key.U;
    public int debugDamageAmount = 1;

    private Color originalColor;
    private bool isDead = false;
    private Coroutine flashRoutine;

    private void Awake()
    {
        // TODO: Check if spriteRenderer is null.
        // If it is, get the SpriteRenderer component from this GameObject.
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



        // TODO: Check that spriteRenderer is not null.
        // If it exists, store its current colour in originalColor.
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }



        // TODO: Check if audioSource is null.
        // If it is, get the AudioSource component from this GameObject.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }



        // TODO: Check if playerExperience is null.
        // If it is, find the PlayerExperience component in the scene.
        if (playerExperience == null)
        {
            playerExperience = FindFirstObjectByType<PlayerExperience>();
        }



        // TODO: Check if playerMoney is null.
        // If it is, find the PlayerMoney component in the scene.
        if (playerMoney == null)
        {
            playerMoney = FindFirstObjectByType<PlayerMoney>();
        }



        // TODO: Check if playerKills is null.
        // If it is, find the PlayerKills component in the scene.
        if (playerKills == null)
        {
            playerKills = FindFirstObjectByType<PlayerKills>();
        }


    }

    private void Start()
    {
        currentHP = maxHP;

        if (showDebugLogs == true)
        {
            Debug.Log("EnemyHealth (" + name + "): Started with HP " + currentHP + "/" + maxHP);
        }
    }

    private void Update()
    {
        HandleDebugInput();
    }

    private void HandleDebugInput()
    {
        if (enableDebugKeys == false)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[debugDamageKey].wasPressedThisFrame)
        {
            TakeDamage(debugDamageAmount);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead == true)
        {
            return;
        }

        // TODO: Subtract amount from the enemy's current health.
        currentHP -= amount;



        // TODO: Check whether currentHP is below 0.
        // If it is, set currentHP to 0.
        if (currentHP < 0)
        {
            currentHP = 0;
        }



        // TODO: If debug logs are enabled,
        // print the damage taken and current health.
        if (showDebugLogs == true)
        {
            Debug.Log("EnemyHealth Took Damage: "+ name + " for " + amount + ", Current HP: " + currentHP);
        }



        // TODO: Call the function that plays the enemy's hit feedback.
        PlayHitFeedback();



        // TODO: Check whether the enemy's current health is 0 or less.
        // If it is, call the Die function.
        if (currentHP <= 0)
        {
            Die();
        }


    }

    private void PlayHitFeedback()
    {
        // Only start the hit flash if one is not already running.
        if (useHitFlash == true && spriteRenderer != null && flashRoutine == null)
        {
            flashRoutine = StartCoroutine(FlashOnHit());
        }

        // TODO: Check that both audioSource and hitSound are not null.
        // If they exist, play hitSound using the AudioSource.
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }


    }

    private IEnumerator FlashOnHit()
    {
        if (spriteRenderer == null)
        {
            flashRoutine = null;
            yield break;
        }

        // TODO: Change the SpriteRenderer's colour to the hit flash colour.
        if (useHitFlash == true && spriteRenderer != null)
        {
            spriteRenderer.color = hitFlashColor;
        }



        yield return new WaitForSeconds(flashDuration);

        // TODO: Check that spriteRenderer is not null.
        // If it exists, change its colour back to originalColor.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }



        // Allow the hit flash to be started again.
        flashRoutine = null;
    }

    private void Die()
    {
        // TODO: Check whether the enemy is already dead.
        // If it is, stop the function.
        if (isDead == true)
        {
            return;
        }



        // TODO: Set isDead to true.
        isDead = true;



        // TODO: If debug logs are enabled,
        // print that the enemy has died.
        if (showDebugLogs == true)
        {
            Debug.Log("Enemy " + name + " Has Died");
        }



        // TODO: Call the function that awards the player's rewards.
        AwardRewards();



        // TODO: Call the function that drops coins.
        DropCoins();



        // TODO: Call the function that plays the death sound.
        PlayDeathSound();


        // Invoke any functions connected to the On Die event in the Inspector.
        // We will look at how UnityEvents work in more detail in later weeks.
        if (onDie != null)
        {
            onDie.Invoke();
        }

        // Remove the enemy from the game.
        Destroy(gameObject);
    }

    private void AwardRewards()
    {
        // Example: Check that the PlayerExperience reference exists
        // before calling one of its functions.
        if (playerExperience != null)
        {
            playerExperience.AddXP(xpReward);
        }

        // TODO: Check that playerMoney is not null.
        // If it exists, call AddMoney and give it moneyReward.
        if (playerMoney != null)
        {
            playerMoney.AddMoney(moneyReward);
        }



        // TODO: Check that playerKills is not null.
        // If it exists, call AddKills and give it killReward.
        if (playerKills != null)
        {
            playerKills.AddKills(killReward);
        }

    }

    private void DropCoins()
    {
        if (coinPrefab == null)
        {
            return;
        }

        if (coinsToDrop <= 0)
        {
            return;
        }

        // TODO: Generate a random value between 0 and 1.
        float roll = Random.Range(0f, 1f);
        

        // TODO: Check whether the roll is more than the drop chance
        // to see if the enemy successfully drops coins.
        if (roll > coinDropChance)
        {
            if (showDebugLogs == true)
            {
                Debug.Log("EnemyHealth (" + name + "): Coin drop failed. Roll = " + roll);
            }

            return;
        }

        // Create the requested number of coins.
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector2 offset = Random.insideUnitCircle * coinSpreadRadius;

            Vector3 spawnPosition = transform.position + (Vector3)offset;

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        if (showDebugLogs == true)
        {
            Debug.Log("EnemyHealth (" + name + "): Dropped " + coinsToDrop + " coin(s).");
        }
    }

    private void PlayDeathSound()
    {
        // TODO: Check that both audioSource and deathSound are not null.
        // If they exist, play deathSound using the AudioSource.
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }


    }
}