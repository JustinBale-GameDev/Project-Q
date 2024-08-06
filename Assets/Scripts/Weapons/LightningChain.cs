using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : MonoBehaviour
{
	public GameObject lightningPrefab; // Prefab for the lightning effect
	public GameObject damageNumberPrefab; // Prefab for the damage number
	public float shootingInterval = 2f; // Time between shots
	public int chainDamage = 10; // Damage dealt by the chain
	public int maxChainJumps = 3; // Maximum number of chain jumps
	public float initialChainRange = 5f; // Maximum range for the initial chain jump
	public float jumpDistance = 3f; // Maximum range for subsequent chain jumps

	private float shootTimer; // Timer to track shooting intervals
	private Transform player; // Reference to the player

	void Start()
	{
		StartCoroutine(FindPlayer());
	}

	void Update()
	{
		if (player == null) return;

		// Increment the timer by the time passed since last frame
		shootTimer += Time.deltaTime;

		// If the timer exceeds the shooting interval, shoot a lightning chain
		if (shootTimer >= shootingInterval)
		{
			ShootLightningChain();
			shootTimer = 0f; // Reset the timer
		}
	}

	void ShootLightningChain()
	{
		GameObject initialTarget = FindNearestEnemyWithinRange(player.position, initialChainRange);
		if (initialTarget != null)
		{
			Debug.Log("Enemy found: SHooting Lightning");
			StartCoroutine(ChainLightning(player.position, initialTarget, maxChainJumps));
		}
	}

	IEnumerator ChainLightning(Vector3 startPosition, GameObject currentTarget, int remainingJumps)
	{
		if (currentTarget == null || remainingJumps <= 0) yield break;

		// Instantiate the lightning effect and draw the line
		Vector3 endPosition = currentTarget.transform.position;
		DrawLightningLine(startPosition, endPosition);

		// Deal damage to the current target
		currentTarget.GetComponent<Enemy_Health>().ReduceHealth(chainDamage);

		// Show damage number
		Instantiate(damageNumberPrefab, currentTarget.transform.position, Quaternion.identity);

		// Find the next nearest enemy within jump distance
		GameObject nextTarget = FindNearestEnemyWithinRange(endPosition, jumpDistance);

		// Wait for a short duration to simulate the speed of the lightning chain
		yield return new WaitForSeconds(0.1f);

		// Continue the chain if a valid next target is found
		if (nextTarget != null)
		{
			StartCoroutine(ChainLightning(endPosition, nextTarget, remainingJumps - 1));
		}
	}

	void DrawLightningLine(Vector3 start, Vector3 end)
	{
		//GameObject lightning = Instantiate(lightningPrefab);
		//LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();
		//lineRenderer.SetPosition(0, start);
		//lineRenderer.SetPosition(1, end);
		//Destroy(lightning, 0.1f); // Destroy the lightning effect after a short duration


		GameObject lightning = Instantiate(lightningPrefab);
		LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();

		int segments = 20; // Number of segments in the line
		lineRenderer.positionCount = segments + 1; // Number of points in the line

		float noiseAmount = 0.5f; // Adjust this to make the lightning more or less jagged

		for (int i = 0; i <= segments; i++)
		{
			float t = (float)i / segments;
			Vector3 point = Vector3.Lerp(start, end, t);

			// Add noise to the point
			float noiseX = Random.Range(-noiseAmount, noiseAmount);
			float noiseY = Random.Range(-noiseAmount, noiseAmount);
			point += new Vector3(noiseX, noiseY, 0);

			lineRenderer.SetPosition(i, point);
		}

		Destroy(lightning, 0.1f); // Destroy the lightning effect after a short duration
	}

	GameObject FindNearestEnemyWithinRange(Vector3 position, float range)
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject nearestEnemy = null;
		float minDistance = range;

		foreach (GameObject enemy in enemies)
		{
			if (enemy.activeInHierarchy)
			{
				float distance = Vector3.Distance(position, enemy.transform.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestEnemy = enemy;
				}
			}
		}

		return nearestEnemy;
	}

	IEnumerator FindPlayer()
	{
		while (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player")?.transform;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
