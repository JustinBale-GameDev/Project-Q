using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Aim : MonoBehaviour
{
	[SerializeField]
	private InputActionAsset inputActions;
	[SerializeField]
	private SpriteRenderer playerSprite;
	[SerializeField]
	private SpriteRenderer weaponSprite; // TODO: The spriteRenderer must always be assigned to the correct weapon. Its currently a manual process in the inspector
	[SerializeField]
	private Animator animator;
	//[SerializeField]
	//private Animator weaponAnimator;
	[SerializeField]

	private Weapon_1 weapon; // Might change if there is more than one weapon
	private InputAction fireAction;
	private Camera mainCam;
	private Vector3 mousePos;
	private int facingDirection = 1;

	private void Awake()
	{
		fireAction = inputActions.FindActionMap("Gameplay").FindAction("Fire_Weapon");
	}

	void Start()
	{
		mainCam = Camera.main;
	}


	void Update()
	{
		AimAndFlipPlayer();
		ControlWeaponSprite();
		HandleShooting();
	}

	// Method to aim the player towards the mouse and flip the player sprite if needed
	private void AimAndFlipPlayer()
	{
		mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePos - transform.position;
		float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, rotZ);

		if (mousePos.x < transform.position.x && facingDirection == 1)
		{
			FlipPlayer();
		}
		else if (mousePos.x > transform.position.x && facingDirection == -1)
		{
			FlipPlayer();
		}

		// Change between forward facing or backward facing
		animator.SetBool("FacingForward", mousePos.y < playerSprite.transform.position.y);
	}

	// Method to flip the player sprite
	public void FlipPlayer()
	{
		facingDirection *= -1;
		Vector3 newScale = playerSprite.transform.localScale;
		newScale.x *= -1;
		playerSprite.transform.localScale = newScale;
	}

	// Method to control the weapon sprite's rotation and sorting order
	private void ControlWeaponSprite()
	{
		Vector2 direction = (mousePos - transform.position).normalized;
		weaponSprite.transform.right = direction;

		Vector2 scale = weaponSprite.transform.localScale;
		scale.y = mousePos.x < transform.position.x ? -1 : 1;
		weaponSprite.transform.localScale = scale;

		weaponSprite.sortingOrder = transform.rotation.z > 0 ? playerSprite.sortingOrder - 1 : playerSprite.sortingOrder + 1;
	}

	// Method to handle shooting mechanics and firing rate
	private void HandleShooting()
	{
		if (fireAction.IsPressed())
		{
			weapon.Fire();
		}
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
