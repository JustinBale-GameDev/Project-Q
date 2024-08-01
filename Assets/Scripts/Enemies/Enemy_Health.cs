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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReduceHealth(int damage)
    {
		currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Reset current health
            currentHealth = maxHealth;

			DropExperienceOrb();

			//Debug.Log("Enemy defeated.");
			this.gameObject.SetActive(false);
		}
    }

	private void DropExperienceOrb()
	{
		Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
	}
}
