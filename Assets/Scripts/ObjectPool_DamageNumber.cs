using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_DamageNumber : MonoBehaviour
{
	public static ObjectPool_DamageNumber Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	[SerializeField] 
	private GameObject damageNumberPrefab;
	public GameObject[] damageNumberPool;
	public int poolCount = 100; // Will need to asjust and test performance
	public int currentIndex = 0;

	// Reference to the parent for pooling
	private GameObject projectilePoolParent;

	private void Start()
	{
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - DamageNumbers");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - DamageNumbers");
		}

		damageNumberPool = new GameObject[poolCount];
		for (int i = 0; i < poolCount; i++)
		{
			damageNumberPool[i] = Instantiate(damageNumberPrefab);
			damageNumberPool[i].transform.SetParent(projectilePoolParent.transform);
			damageNumberPool[i].SetActive(false);
		}
	}

	public void IncreaseIndex()
	{
		currentIndex = (currentIndex + 1) % poolCount;
	}

	public void SpawnDamageNumber(Vector3 position, int damage)
	{
		GameObject damageNumber = damageNumberPool[currentIndex];

		//damageNumber.transform.position = position;
		// Adjust the y position to display the damage number above the enemy
		Vector3 adjustedPosition = new Vector3(position.x, position.y + 0.5f, position.z);
		damageNumber.transform.position = adjustedPosition;

		damageNumber.GetComponent<DamageNumber>().SetDamage(damage);
		damageNumber.SetActive(true);
		IncreaseIndex();
	}
}
