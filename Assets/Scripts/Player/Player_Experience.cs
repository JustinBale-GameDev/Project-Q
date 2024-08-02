using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Experience : MonoBehaviour
{
	public int experiencePoints = 0;
	public int level = 1;
	public int experienceToNextLevel = 10;
	public Slider experienceBar;

	public void AddExperience(int amount)
	{
		experiencePoints += amount;

		UpdateExperienceBar();

		if (experiencePoints >= experienceToNextLevel)
		{
			LevelUp();
		}
	}

	private void UpdateExperienceBar()
	{
		experienceBar.value = (float)experiencePoints / experienceToNextLevel;
	}

	private void LevelUp()
	{
		experiencePoints -= experienceToNextLevel;
		level++;
		experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.3f); // Increase the required experience to level up

		UpdateExperienceBar();

		// play a level-up sound or effect here
		//Debug.Log("Level Up! Current level: " + level);
	}
}
