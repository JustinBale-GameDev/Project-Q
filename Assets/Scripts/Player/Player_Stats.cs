using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{

	public static Player_Stats Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	public int health;
	public int maxHealth;
	public int damage;
	public int knockback;
	public int knockbackChance;
	public int critChance;
	public float speed;
	public float fireRate;


	public void TakeDamage(int amount)
	{
		health -= amount;
		if (health <= 0)
		{
			health = 0;

			// Handle player death, respawn, game over
		}
	}

	public void Heal(int amount)
	{
		health += amount;
		if (health > maxHealth)
		{
			health = maxHealth;
		}
	}

	public void IncreaseStat(string statName, int amount)
	{
		switch (statName)
		{
			case "health":
				health += amount;
				if (health > maxHealth)
				{
					health = maxHealth;
				}
				break;
			case "maxHealth":
				maxHealth += amount;
				break;
			case "damage":
				damage += amount;
				break;
			case "speed":
				speed += amount;
				break;
			case "fireRate":
				fireRate -= amount; // Decrease to increase fire rate
				break;
			default:
				Debug.LogWarning("Stat not recognized.");
				break;
		}
	}
}
