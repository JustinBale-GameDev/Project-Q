using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
	public static Player_Health Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public Image healthUI;
	public float maxHealth;
	[SerializeField] private float currentHealth;
	public bool isDead;
	public float regenRate;
	public float invincibilityDuration = 1f; // Time in seconds for invincibility
	private bool isInvincible;
	private float lastDamageTime;

	// Sprite Renderer
	[SerializeField] private SpriteRenderer spriteRenderer;
	private Color originalColor;


	void Start()
	{
		currentHealth = maxHealth;
		isDead = false;
		isInvincible = false;
		UpdateHealthUI();

		originalColor = spriteRenderer.color;
	}

	void Update()
	{
		if (!isDead && currentHealth < maxHealth)
		{
			RegenerateHealth();
		}

		// Check if invincibility has worn off
		if (isInvincible && Time.time > lastDamageTime + invincibilityDuration)
		{
			isInvincible = false;
			ResetColor();
		}
	}

	public void ApplyDamage(float damage)
	{
		if (isInvincible)
		{
			return; // Ignore damage if invincible
		}

		currentHealth -= damage;
		currentHealth = Mathf.Max(currentHealth, 0);
		UpdateHealthUI();

		// Set invincibility
		isInvincible = true;
		lastDamageTime = Time.time;

		// Check if health has dropped to 0 or below
		if (currentHealth <= 0 && !isDead)
		{
			isDead = true;
		}

		// Start color change coroutine
		if (spriteRenderer != null)
		{
			//StartCoroutine(DamageColorChange());
			StartCoroutine(DamageFlashEffect());
		}
	}

	private void UpdateHealthUI()
	{
		float healthScale = Mathf.Clamp(1 - currentHealth / maxHealth, 0, 1);
		healthUI.transform.localScale = new Vector3(healthScale, 1, 1);
	}

	private void RegenerateHealth()
	{
		currentHealth += regenRate * Time.deltaTime; // health per second
		currentHealth = Mathf.Min(currentHealth, maxHealth); // health does not exceed maxHealth
		UpdateHealthUI();
	}

	public void Heal(int healthGain)
	{
		currentHealth += healthGain;
		currentHealth = Mathf.Min(currentHealth, maxHealth);
		UpdateHealthUI();
	}

	private IEnumerator DamageColorChange()
	{
		spriteRenderer.color = Color.red;
		float elapsedTime = 0f;

		while (elapsedTime < invincibilityDuration)
		{
			spriteRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / invincibilityDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		spriteRenderer.color = originalColor;
	}
	private IEnumerator DamageFlashEffect()
	{
		float flashInterval = 0.1f; // Time between flashes
		float elapsedTime = 0f;

		while (elapsedTime < invincibilityDuration)
		{
			spriteRenderer.color = Color.red;
			yield return new WaitForSeconds(flashInterval);
			spriteRenderer.color = originalColor;
			yield return new WaitForSeconds(flashInterval);
			elapsedTime += flashInterval * 2; // Since we have two WaitForSeconds in each loop
		}

		spriteRenderer.color = originalColor;
	}

	private void ResetColor()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = originalColor;
		}
	}
}
