using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Transform player;

	public float speed;
	private int facingDIrection = -1;

	// Damage variables
	public float damageAmount = 10f;
	public float damageDistanceThreshold = 1f;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if (player != null)
		{
			if (player.position.x > transform.position.x && facingDIrection == -1 ||
				player.position.x < transform.position.x && facingDIrection == 1)
			{
				Flip();
			}

			// Chase player
			Vector2 direction = (player.position - transform.position).normalized;
			rb.velocity = direction * speed;

			// When close enough to player, apply damage
			float distanceToPlayer = Vector2.Distance(transform.position, player.position);
			if (distanceToPlayer <= damageDistanceThreshold)
			{
				Player_Health.Instance.ApplyDamage(damageAmount);
			}
		}
	}

	void Flip()
	{
		facingDIrection *= -1;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}
}
