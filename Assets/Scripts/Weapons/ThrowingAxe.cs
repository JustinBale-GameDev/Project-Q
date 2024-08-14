using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ThrowingAxe : MonoBehaviour
{
	public GameObject axePrefab; // Prefab for the axe
	public int numberOfAxes; // Number of axes to throw
	public float throwInterval; // Time between throws
	public int damage; // Damage each axe does
	public float speed; // Speed of the axes
	public float returnSpeed; // Speed of axes when returning to player
	public float axeLifetime; // Time before axe starts returning

	private float throwTimer; // Timer to track throw intervals

	// Gameobject pooling variables
	private GameObject[] axes;
	private GameObject projectilePoolParent;
	private int currentIndex = 0;
	public int poolCount = 25;

	void Start()
	{
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - Axes");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - Axes");
		}

		axes = new GameObject[poolCount];
		for (int i = 0; i < poolCount; i++)
		{
			axes[i] = Instantiate(axePrefab);
			axes[i].transform.SetParent(projectilePoolParent.transform);
			axes[i].SetActive(false);
		}
	}

	void Update()
	{
		// Increment the timer by the time passed since last frame
		throwTimer += Time.deltaTime;

		// If the timer exceeds the throw interval, throw the axes
		if (throwTimer >= throwInterval)
		{
			ThrowAxes();
			throwTimer = 0f; // Reset the timer
		}
	}

	void ThrowAxes()
	{
		// Loop through the number of axes
		for (int i = 0; i < numberOfAxes; i++)
		{
			// Calculate the angle at which to throw the axe
			float angle = i * Mathf.PI * 2 / numberOfAxes;
			Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

			// Instantiate an axe and set its initial position to the player's position
			//GameObject axe = Instantiate(axePrefab, transform.position, Quaternion.identity);

			GameObject currentProjectile = axes[currentIndex];
			currentProjectile.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
			currentProjectile.SetActive(true);

			// Initialize the axe with direction, speed, return speed, damage, and player reference
			currentProjectile.GetComponent<Axe>().Initialize(direction, speed, returnSpeed, damage, gameObject, axeLifetime);

			currentIndex = (currentIndex + 1) % poolCount;
		}
	}
}
