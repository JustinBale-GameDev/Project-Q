using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Transform player;

	public float speed;
	private int facingDIrection = -1;

	private EnemyState enemyState;

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
		}
		else
		{
			//Debug.Log("Player not found by: " + this.gameObject.name);
		}
	}

	void Flip()
	{
		facingDIrection *= -1;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}
}


public enum EnemyState
{
	idle,
	chasing,
	attacking
}
