using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
	public float duration = 1.5f;
	private float timer;
	private TMP_Text text;
	private Animator animator;  // Reference to the Animator component

	void Awake()
	{
		text = GetComponent<TextMeshPro>();
		animator = GetComponent<Animator>();  // Initialize the animator reference
	}

	void OnEnable()
	{
		Initialize();  // Ensure initialization happens when object is enabled
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= duration)
		{
			this.gameObject.SetActive(false);
		}
	}

	public void Initialize()
	{
		timer = 0f;
		text.alpha = 1f;  // Reset the alpha of the text
		if (animator != null)
		{
			animator.Play("PopupAnimation", -1, 0f);  // Reset animation
		}
	}

	public void SetDamage(int damage)
	{
		text.text = damage.ToString();
	}
}
