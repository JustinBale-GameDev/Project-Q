using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage;
	public GameObject damageNumberPrefab;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Reduce enemy health
		Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
		if (enemyHealth != null)
		{
			enemyHealth.ReduceHealth(damage);

			// Instantiate damage number prefab
			GameObject damageNumber = Instantiate(damageNumberPrefab, collision.transform.position, Quaternion.identity);
			damageNumber.GetComponent<DamageNumber>().SetDamage(damage);

			// Disable projectile
			this.gameObject.SetActive(false);
		}

		if (collision.gameObject.CompareTag("Border"))
		{
			// Disable projectile
			this.gameObject.SetActive(false);
		}

		// Disable projectile
		//this.gameObject.SetActive(false);
	}
}
