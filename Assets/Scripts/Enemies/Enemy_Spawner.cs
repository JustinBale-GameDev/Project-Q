using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
	public GameObject enemyPrefab; // Assign the enemy prefab in the inspector
	public Transform[] spawnPoints; // Assign spawn points in the inspector
	public float spawnInterval = 2f; // Time between spawns
	public int enemyPoolSize = 50;

	private float timer;
	private GameObject[] enemies;
	private int currentEnemyIndex;

	void Start()
	{
		enemies = new GameObject[enemyPoolSize];
		for (int i = 0; i < enemyPoolSize; i++)
		{
			enemies[i] = Instantiate(enemyPrefab);
			enemies[i].SetActive(false);
		}
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= spawnInterval)
		{
			SpawnEnemy();
			timer = 0f;
		}
	}

	void SpawnEnemy()
	{
		int randomIndex = Random.Range(0, spawnPoints.Length);
		GameObject enemy = enemies[currentEnemyIndex];
		enemy.transform.position = spawnPoints[randomIndex].position;
		enemy.SetActive(true);
		currentEnemyIndex = (currentEnemyIndex + 1) % enemyPoolSize;
	}
}
