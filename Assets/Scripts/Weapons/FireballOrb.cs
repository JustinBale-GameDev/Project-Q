using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballOrb : MonoBehaviour
{
	public GameObject fireballPrefab; // Prefab for the fireball
	public float shootingInterval = 2f; // Time between shots
	public float fireballSpeed = 5f; // Speed of the fireball
	public int fireballDamage = 10; // Damage dealt by the fireball
	public float orbDistance = 2f; // Distance from the player
	public float floatSpeed = 2f; // Speed of the floating movement
	public float floatAmplitude = 0.5f; // Amplitude of the floating movement
	public float sideToSideSpeed = 1f; // Speed of the side-to-side movement
	public float sideToSideAmplitude = 1f; // Amplitude of the side-to-side movement
	public int numberOfProjectiles = 1; // Number of fireball projectiles to shoot

	private float shootTimer; // Timer to track shooting intervals
	private Transform player; // Reference to the player

	// Gameobject pooling variables
	private GameObject[] fireBalls;
	private GameObject projectilePoolParent;
	private int currentIndex = 0;
	public int poolCount = 75;

	void Start()
	{
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - Fireballs");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - Fireballs");
		}

		fireBalls = new GameObject[poolCount];
		for (int i = 0; i < poolCount; i++)
		{
			fireBalls[i] = Instantiate(fireballPrefab);
			fireBalls[i].transform.SetParent(projectilePoolParent.transform);
			fireBalls[i].SetActive(false);
		}

		StartCoroutine(FindPlayer());
	}

	void Update()
	{
		if (player == null) return;

		// Calculate the floating effect
		Vector3 floatEffect = new Vector3(Mathf.Sin(Time.time * sideToSideSpeed) * sideToSideAmplitude, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0);

		// Position the orb around the player with floating and side-to-side effect
		transform.position = player.position + floatEffect + new Vector3(0, orbDistance, 0);

		// Increment the timer by the time passed since last frame
		shootTimer += Time.deltaTime;

		// If the timer exceeds the shooting interval, shoot a fireball
		if (shootTimer >= shootingInterval)
		{
			ShootFireball();
			shootTimer = 0f; // Reset the timer
		}
	}

	void ShootFireball()
	{
		List<GameObject> nearestEnemies = FindNearestEnemies(numberOfProjectiles);
		foreach (GameObject enemy in nearestEnemies)
		{
			if (enemy != null)
			{
				Vector3 direction = (enemy.transform.position - transform.position).normalized;
				//GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

				GameObject currentProjectile = fireBalls[currentIndex];
				currentProjectile.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
				currentProjectile.SetActive(true);

				currentProjectile.GetComponent<Fireball>().Initialize(direction, fireballSpeed, fireballDamage);

				currentIndex = (currentIndex + 1) % poolCount;
			}
		}
	}

	List<GameObject> FindNearestEnemies(int count)
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		List<GameObject> nearestEnemies = new List<GameObject>();

		// Sort enemies by distance
		System.Array.Sort(enemies, (a, b) => Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));

		// Get the nearest 'count' enemies
		for (int i = 0; i < Mathf.Min(count, enemies.Length); i++)
		{
			if (enemies[i].activeInHierarchy)
			{
				nearestEnemies.Add(enemies[i]);
			}
		}

		return nearestEnemies;
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
