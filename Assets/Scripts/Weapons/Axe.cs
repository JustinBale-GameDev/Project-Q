using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
	private Vector3 direction; // Initial direction of the axe
	private float speed; // Speed of the axe
	private float returnSpeed; // Speed of the axe when returning to the player
	private int damage; // Damage the axe deals
	private GameObject player; // Reference to the player
	private bool returning = false; // Whether the axe is returning to the player
	private float lifetime; // Time before the axe returns
	private float timer; // Timer to track axe lifetime

	public GameObject damageNumberPrefab;
	public float rotationSpeed;

	// Initialize method to set up the axe properties
	public void Initialize(Vector3 direction, float speed, float returnSpeed, int damage, GameObject player, float lifetime)
	{
		this.direction = direction;
		this.speed = speed;
		this.returnSpeed = returnSpeed;
		this.damage = damage;
		this.player = player;
		this.lifetime = lifetime;
	}

	void Update()
	{
		// Increment the timer by the time passed since last frame
		timer += Time.deltaTime;

		// If the axe is not returning, move it in the initial direction
		if (!returning)
		{
			transform.position += direction * speed * Time.deltaTime;

			// If the timer exceeds the lifetime, switch to returning
			if (timer >= lifetime)
			{
				returning = true;
			}
		}
		else
		{
			// Move the axe back towards the player
			Vector3 returnDirection = (player.transform.position - transform.position).normalized;
			transform.position += returnDirection * returnSpeed * Time.deltaTime;

			// When close enough to player, disable gameobject
			if (Vector3.Distance(transform.position, player.transform.position) < 2f)
			{
				// Disable axe
				this.gameObject.SetActive(false);
			}
		}

		RotateAxe();
	}

	public void RotateAxe()
	{
		transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
	}

	// Handle collision detection with enemies
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
			if (enemyHealth != null)
			{
				enemyHealth.ReduceHealth(damage);

				// Use the object pool to spawn a damage number
				ObjectPool_DamageNumber.Instance.SpawnDamageNumber(collision.transform.position, Player_Stats.Instance.damage);
			}
		}
	}
}
