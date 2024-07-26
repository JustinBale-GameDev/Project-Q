using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Return_To_Map_Button : MonoBehaviour
{
	public void ReturnToMap()
	{
		SceneManager.LoadScene("WorldMap1");
	}
}
