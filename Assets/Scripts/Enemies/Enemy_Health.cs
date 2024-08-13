using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
	public GameObject experienceOrbPrefab;

	// Start is called before the first frame update
	void Start()
    {
        currentHealth = maxHealth;
    }

    public void ReduceHealth(int damage)
    {
		currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Enabled experience orb
			ObjectPool_ExperienceOrb.Instance.SpawnExperienceOrb(transform.position);

			// Reset current health
			currentHealth = maxHealth;

			// Disable object
			this.gameObject.SetActive(false);
		}
    }
}
