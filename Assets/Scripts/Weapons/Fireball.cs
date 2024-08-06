using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	private Vector3 direction; // Direction of the fireball
	private float speed; // Speed of the fireball
	private int damage; // Damage dealt by the fireball

	public GameObject damageNumberPrefab; // Prefab for displaying damage numbers

	// Initialize the fireball's properties
	public void Initialize(Vector3 direction, float speed, int damage)
	{
		this.direction = direction;
		this.speed = speed;
		this.damage = damage;
	}

	void Update()
	{
		// Move the fireball in the specified direction
		transform.position += direction * speed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
			if (enemyHealth != null)
			{
				enemyHealth.ReduceHealth(damage);

				// Instantiate damage number prefab
				GameObject damageNumber = Instantiate(damageNumberPrefab, collision.transform.position, Quaternion.identity);
				damageNumber.GetComponent<DamageNumber>().SetDamage(damage);
			}

			// Destroy the fireball on impact
			Destroy(gameObject);
		}
	}
}
