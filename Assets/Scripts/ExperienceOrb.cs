using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
	public float moveSpeed = 8f;
	public int experienceGranted;
	private Transform playerTransform;
	private bool isMovingToPlayer = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerTransform = collision.transform;
			isMovingToPlayer = true;
		}
	}

	private void Update()
	{
		if (isMovingToPlayer)
		{
			// Move towards the player
			transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

			// Check if reached player
			if (Vector2.Distance(transform.position, playerTransform.position) < 0.5f)
			{
				// Add experience points to player
				playerTransform.GetComponent<Player_Experience>().AddExperience(experienceGranted);
				Destroy(gameObject);
			}
		}
	}
}
