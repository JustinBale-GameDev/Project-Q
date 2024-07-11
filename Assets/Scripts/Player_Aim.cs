using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Aim : MonoBehaviour
{
	[SerializeField]
	private InputActionAsset inputActions;
	[SerializeField]
	private GameObject playerSprite;

	[SerializeField]
	private SpriteRenderer spriteRenderer; // Current weapon sprite renderer (THis will eventually be a list and will select the one that is active)
	

	private InputAction fireAction;
	private Camera mainCam;
	private Vector3 mousePos;
	public GameObject bullet;
	public Transform bulletTransform;
	public bool canFire;
	private float timer;
	public float timerBetweenFiring;

	private int facingDirection = 1;



	private void Awake()
	{
		fireAction = inputActions.FindActionMap("Gameplay").FindAction("Fire_Weapon");
	}

	void Start()
	{
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}


	void Update()
	{
		mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		Vector3 rotation = mousePos - transform.position;
		float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, rotZ);

		// Flip player sprite based on mouse position
		if (mousePos.x < transform.position.x && facingDirection == 1) // Flip the player sprite if the mouse if on the left of the player and the player is facing right
		{
			FlipPlayer();
		}
		else if (mousePos.x > transform.position.x && facingDirection == -1) // Flip the player sprite if the mouse is on the right of the player and the player is facing left
		{
			FlipPlayer();
		}

		//// Flip weapon so it does not go upside down
		//if (mousePos.x < transform.position.x)
		//{
		//	//Vector3 newScale = spriteRenderer.transform.localScale;
		//	//newScale.x *= -1;
		//	//spriteRenderer.transform.localScale = newScale;
		//	spriteRenderer.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		//}


		// Weapon will be behind player or in front depending on where the mouse is
		if (transform.rotation.z > 0)
		{
			spriteRenderer.sortingOrder = 0;
		}
		else if (transform.rotation.z < 0)
		{
			spriteRenderer.sortingOrder = 5;
		}

		// Shooting timer
		if (!canFire)
		{
			timer += Time.deltaTime;
			if (timer > timerBetweenFiring)
			{
				canFire = true;
				timer = 0;
			}
		}

		// Firing weapong
		if (fireAction.WasPressedThisFrame() && canFire)
		{
			canFire = false;
			Instantiate(bullet, bulletTransform.position, Quaternion.identity);
		}
	}

	public void FlipPlayer()
	{
		//playerSprite.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		facingDirection *= -1;
		Vector3 newScale = playerSprite.transform.localScale;
		newScale.x *= -1;
		playerSprite.transform.localScale = newScale;
	}

	private void OnEnable()
	{
		fireAction.Enable();
	}
	private void OnDisable()
	{
		fireAction.Disable();
	}
}
