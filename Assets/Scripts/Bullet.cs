using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public GameObject damageNumberPrefab;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Collision with enemy
		if (collision.gameObject.CompareTag("Enemy"))
		{
			// Reduce enemy health
			Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
			if (enemyHealth != null)
			{
				enemyHealth.ReduceHealth(Player_Stats.Instance.damage);

				// Instantiate damage number prefab
				GameObject damageNumber = Instantiate(damageNumberPrefab, collision.transform.position, Quaternion.identity);
				damageNumber.GetComponent<DamageNumber>().SetDamage(Player_Stats.Instance.damage);

				// Disable projectile
				this.gameObject.SetActive(false);
			}
		}
		
		// Collision with border (Change to environment)
		if (collision.gameObject.CompareTag("Border"))
		{
			// Disable projectile
			this.gameObject.SetActive(false);
		}
	}
}
