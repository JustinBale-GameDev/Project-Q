using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Player : MonoBehaviour
{
	private void Awake()
	{
		if (!SceneManager.GetSceneByName("Player").isLoaded)
		{
			SceneManager.LoadScene("Player", LoadSceneMode.Additive);
		}
	}
}
