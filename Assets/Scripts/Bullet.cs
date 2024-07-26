using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Reduce enemy health
		Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
		if (enemyHealth != null)
		{
			enemyHealth.ReduceHealth(damage);
		}

		// Disable projectile
		this.gameObject.SetActive(false);
	}
}
