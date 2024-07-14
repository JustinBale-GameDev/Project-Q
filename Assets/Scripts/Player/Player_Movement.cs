using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private InputActionAsset inputActions;
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private Animator anim;

	private InputAction moveAction;
	private Vector2 moveValue;

	[Header("Changable Values")]
	public float speed = 5;


	private void Awake()
	{
		moveAction = inputActions.FindActionMap("Gameplay").FindAction("Movement");
	}

	// FixedUpdate is called 50x frame
	void FixedUpdate()
	{
		moveValue = moveAction.ReadValue<Vector2>();

		anim.SetFloat("Horizontal", Mathf.Abs(moveValue.x)); // Convert x value into postive
		anim.SetFloat("Vertical", Mathf.Abs(moveValue.y));// Convert y value into postive

		rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveValue);
	}

	private void OnEnable()
	{
		moveAction.Enable();
	}
	private void OnDisable()
	{
		moveAction.Disable();
	}
}
