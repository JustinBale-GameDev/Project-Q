using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_1 : MonoBehaviour // Bow and arrow
{
	public int poolCount = 40;

	[SerializeField]
	private GameObject projectile;
	[SerializeField]
	private Transform weaponTransform;

	private GameObject[] projectiles;
	private int currentIndex = 0;
	private float timeStamp;

	// Reference to the parent for pooling
	private GameObject projectilePoolParent;


	void Start()
    {
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - Arrows");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - Arrows");
		}

		projectiles = new GameObject[poolCount];
		for (int i = 0; i < poolCount; i++)
		{
			projectiles[i] = Instantiate(projectile);
			projectiles[i].transform.SetParent(projectilePoolParent.transform);
			projectiles[i].SetActive(false);
		}
	}

    public void Fire()
    {
		if (Time.time > timeStamp + Player_Stats.Instance.currentFireRate)
		{
			timeStamp = Time.time;

			GameObject currentProjectile = projectiles[currentIndex];
			//currentProjectile.transform.position = weaponTransform.position;
			//currentProjectile.transform.rotation = weaponTransform.rotation;
			currentProjectile.transform.SetPositionAndRotation(weaponTransform.position, weaponTransform.rotation);
			currentProjectile.SetActive(true);

			Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.velocity = weaponTransform.right * Player_Stats.Instance.currentVelocity;
			}

			currentIndex = (currentIndex + 1) % poolCount;
		}
	}
}
