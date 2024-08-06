using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour // This script exists on an empty gameObject in the scene
{
	[SerializeField] private int enemyPoolSize = 50;
	[SerializeField] private float spawnInterval = 2f; // Time between spawns
	[SerializeField] private float spawnRadius = 10f; // Radius around the player to spawn enemies

	[SerializeField] private GameObject enemyPrefab;
	//[SerializeField] private Transform[] spawnPoints;

	[SerializeField] private bool visualizeSpawnPositions = true; // Toggle visualization

	private GameObject[] enemies;
	private Transform player;
	//private int currentEnemyIndex;
	private float timeStamp;

	// Reference to the parent for pooling
	private GameObject projectilePoolParent;

	void Start()
	{
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - Enemy Slimes");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - Enemy Slimes");
		}

		enemies = new GameObject[enemyPoolSize];
		for (int i = 0; i < enemyPoolSize; i++)
		{
			enemies[i] = Instantiate(enemyPrefab);
			enemies[i].transform.SetParent(projectilePoolParent.transform);
			enemies[i].SetActive(false);
		}

		StartCoroutine(FindPlayer());
	}

	void Update()
	{
		timeStamp += Time.deltaTime;

		if (timeStamp >= spawnInterval)
		{
			SpawnEnemy();
			timeStamp = 0f;
		}
	}

	void SpawnEnemy()
	{
		if (player == null) return;

		for (int i = 0; i < enemyPoolSize; i++)
		{
			if (!enemies[i].activeInHierarchy)
			{
				//Vector2 spawnPosition = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;

				Vector2 spawnPosition = GetSpawnPositionOnEdge();
				enemies[i].transform.position = spawnPosition;
				enemies[i].SetActive(true);
				break;
			}
		}
	}

	Vector2 GetSpawnPositionOnEdge()
	{
		float angle = Random.Range(0f, Mathf.PI * 2);
		Vector2 spawnPosition = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
		return spawnPosition;
	}

	IEnumerator FindPlayer()
	{
		while (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player")?.transform;
			yield return new WaitForSeconds(0.5f);
		}
	}

	void OnDrawGizmos()
	{
		if (visualizeSpawnPositions && player != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(player.position, spawnRadius);
		}
	}
}
