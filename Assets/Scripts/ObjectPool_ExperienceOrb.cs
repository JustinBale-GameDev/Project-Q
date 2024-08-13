using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_ExperienceOrb : MonoBehaviour
{
	public static ObjectPool_ExperienceOrb Instance { get; private set; }

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
	private GameObject experienceOrbPrefab;
	public GameObject[] experienceOrbPool;
	public int poolCount = 100; // Will need to asjust and test performance
	public int currentIndex = 0;

	// Reference to the parent for pooling
	private GameObject projectilePoolParent;

	private void Start()
	{
		// Find or create the parent GameObject for pooling
		projectilePoolParent = GameObject.Find("ObjectPool - ExperienceOrbs");
		if (projectilePoolParent == null)
		{
			projectilePoolParent = new GameObject("ObjectPool - ExperienceOrbs");
		}

		experienceOrbPool = new GameObject[poolCount];
		for (int i = 0; i < poolCount; i++)
		{
			experienceOrbPool[i] = Instantiate(experienceOrbPrefab);
			experienceOrbPool[i].transform.SetParent(projectilePoolParent.transform);
			experienceOrbPool[i].SetActive(false);
		}
	}

	public void IncreaseIndex()
	{
		currentIndex = (currentIndex + 1) % poolCount;
	}

	public void SpawnExperienceOrb(Vector3 position)
	{
		GameObject experienceOrb = experienceOrbPool[currentIndex];
		experienceOrb.transform.position = position;
		experienceOrb.SetActive(true);
		IncreaseIndex();
	}
}
