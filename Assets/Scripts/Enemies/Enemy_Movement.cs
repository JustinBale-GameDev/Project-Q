using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	public float speed;

	private Rigidbody2D rb;
	private Transform player;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if (player != null)
		{
			Vector2 direction = (player.position - transform.position).normalized;
			rb.velocity = direction * speed;
		}
		else
		{
			Debug.Log("Player not found by: " + this.gameObject.name);
		}
	}
}
