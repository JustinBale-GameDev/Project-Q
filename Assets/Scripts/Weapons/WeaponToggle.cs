using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
	public GameObject hammer;
	public GameObject throwingAxe;
	public GameObject fireballOrb;

	public void ToggleHammer()
	{
		hammer.SetActive(!hammer.activeSelf);
	}

	public void ToggleThrowingAxe()
	{
		throwingAxe.SetActive(!throwingAxe.activeSelf);
	}

	public void ToggleFireballOrb()
	{
		fireballOrb.SetActive(!fireballOrb.activeSelf);
	}
}
