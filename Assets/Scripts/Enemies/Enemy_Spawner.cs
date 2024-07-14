using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
	public GameObject enemyPrefab; // Assign the enemy prefab in the inspector
	public Transform[] spawnPoints; // Assign spawn points in the inspector
	public float spawnInterval = 2f; // Time between spawns

	private float timer;

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
		Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
	}
}
