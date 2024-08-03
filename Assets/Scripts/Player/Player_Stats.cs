using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
	public static Player_Stats Instance {  get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject); // Persist across scenes
		}
	}

	// Hard Stats (Persistent across levels)
	[Header("Hard Stats")]
	public int maxHealth = 100;
	public float regenRate = 5f;
	public float invincibilityDuration = 1f;
	public int damage = 10;
	public float fireRate = 0.5f;
	public float velocity = 10f;
	public float speed = 5f;
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  Add other hard stats here (Not implemented yet)
	//public int optionSlotIncrease = 2; // Default to 2, can be increased to 3 or 4
	//public float defense = 5f;
	//public float criticalHitChance = 0.1f; // 10% crit chance
	//public float pickupRadius = 2f;
	//public float resourceCollectionRate = 1f;
	//public float cooldownReduction = 0.1f; // 10% cooldown reduction
	// Knockback


	// Soft Stats / Level-specific stats (Temporarily increased during levels, resetting at the end)
	[Header("Soft Stats")]
	public int currentHealth;
	public float currentFireRate;
	public float currentVelocity;

	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  Add other soft stats here (Not implemented yet)
	//public float levelHealthBonus = 0f;
	//public float levelDamageBonus = 0f;
	//public float levelDefenseBonus = 0f;
	//public float levelSpeedBonus = 0f;
	//public float levelCriticalHitBonus = 0f;
	//public float levelPickupRadiusBonus = 0f;
	//public float levelCooldownReductionBonus = 0f;


	//// Methods to get total values including bonuses
	//public float GetTotalHealth() => maxHealth + levelHealthBonus;
	//public float GetTotalDamage() => damage + levelDamageBonus;
	//public float GetTotalDefense() => defense + levelDefenseBonus;
	//public float GetTotalSpeed() => speed + levelSpeedBonus;
	//public float GetTotalCriticalHitChance() => criticalHitChance + levelCriticalHitBonus;
	//public float GetTotalPickupRadius() => pickupRadius + levelPickupRadiusBonus;
	//public float GetTotalCooldownReduction() => cooldownReduction + levelCooldownReductionBonus;


	//// Methods to modify stats (can be expanded as needed)
	//public void IncreaseHealth(float amount) => maxHealth += amount;
	//public void IncreaseDamage(float amount) => damage += amount;
	//public void IncreaseDefense(float amount) => defense += amount;
	//public void IncreaseSpeed(float amount) => speed += amount;
	//public void IncreaseCriticalHitChance(float amount) => criticalHitChance += amount;
	//public void IncreasePickupRadius(float amount) => pickupRadius += amount;
	//public void IncreaseCooldownReduction(float amount) => cooldownReduction += amount;

	private void Start()
	{
		ResetSoftStats();
	}

	public void ResetSoftStats()
	{
		currentHealth = maxHealth;
		currentFireRate = fireRate;
		currentVelocity = velocity;
	}
}
