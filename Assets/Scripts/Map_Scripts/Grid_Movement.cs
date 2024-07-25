using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grid_Movement : MonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private InputActionAsset inputActions;
	private InputAction moveAction;
	private InputAction switchMovementAction;
	private Vector2 moveValue;
	private Rigidbody2D rb;
	private CircleCollider2D circleCollider;

	public Pathnode currentNode;
	private Pathnode previousNode;
	private Vector3 targetPosition;
	private bool isMovingToNode = false;
	public float moveSpeed = 5f;
	private int facingDirection = 1;



	public bool inWater = false;
	public SpriteRenderer currentSprite;
	public Sprite shipSprite;
	public Sprite playerSprite;
	public Transform shipTransform;
	public GameObject dockUIPanel;

	private void Awake()
	{
		moveAction = inputActions.FindActionMap("GridControls").FindAction("Grid_Movement");
		switchMovementAction = inputActions.FindActionMap("GridControls").FindAction("EnterExitShip");

		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
		circleCollider.enabled = false;
	}

	private void Start()
	{
		if (currentNode != null)
		{
			transform.position = currentNode.transform.position;
		}
		else
		{
			Debug.LogError("Current node is not assigned.");
		}

		currentSprite.sprite = playerSprite;
		dockUIPanel.SetActive(false);
	}

	private void FixedUpdate()
	{
		moveValue = moveAction.ReadValue<Vector2>();

		if (inWater)
		{
			rb.MovePosition(rb.position + (moveSpeed / 2) * Time.fixedDeltaTime * moveValue); // Water movement
		}

		if (moveValue.x < 0 && facingDirection == 1) // Facing right
		{
			FlipSprite();
		}
		else if (moveValue.x > 0 && facingDirection == -1) // Facing left
		{
			FlipSprite();
		}
    }

	public void FlipSprite()
	{
		facingDirection *= -1;
		Vector3 newScale = currentSprite.transform.localScale;
		newScale.x *= -1;
		currentSprite.transform.localScale = newScale;
	}

	void Update()
	{
		// If player is not currently moving to a node, input is detected, and not in water
		if (!isMovingToNode && moveValue != Vector2.zero && !inWater)
		{
			Vector2 direction = moveValue.normalized;
			//Debug.Log(direction.ToString());
			Pathnode targetNode = GetConnectedNode(direction);
			TryMoveToNode(targetNode, direction);
		}

		if (isMovingToNode)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				isMovingToNode = false;
				transform.position = targetPosition;
				//Debug.Log("Reached Target Node");


				// Enable UI for dock node and allow for input to switch to ship
				if (currentNode.isDockNode)
				{
					dockUIPanel.SetActive(true);
				}
			}
		}

		// Check for enter action to switch to ship
		if (dockUIPanel.activeSelf && switchMovementAction.WasPressedThisFrame())
		{
			EnableDisableShipMovement();
		}

		// if inWater = true and distance to dockNode is < 0.01 { enable pannel - press enter - reattch to node (TODO: Make ship sprire appear next to dock)
		
	}

	Pathnode GetConnectedNode(Vector2 direction)
	{
		foreach (Pathnode node in currentNode.connections)
		{
			Vector2 dirToNode = (node.transform.position - currentNode.transform.position).normalized;
			if (Vector2.Dot(dirToNode, direction) > 0.9f)
			{
				return node;
			}
		}
		//Debug.Log("No connected node found in the given direction.");
		return null;
	}

	void TryMoveToNode(Pathnode targetNode, Vector2 inputDirection)
	{
		if (targetNode != null)
		{
			if (!currentNode.locked || IsMovingBack(inputDirection))
			{
				MoveToNode(targetNode);
			}
			else
			{
				//Debug.Log("Cannot move forward past a locked node.");
			}
		}
	}

	bool IsMovingBack(Vector2 inputDirection)
	{
		if (previousNode != null)
		{
			Vector2 dirToPreviousNode = (previousNode.transform.position - currentNode.transform.position).normalized;
			return Vector2.Dot(dirToPreviousNode, inputDirection) > 0.9f;
		}
		return false;
	}

	void MoveToNode(Pathnode targetNode)
	{
		previousNode = currentNode; // Store the current node as previous
		currentNode = targetNode;
		targetPosition = currentNode.transform.position;
		isMovingToNode = true;
		//Debug.Log($"Moving to node at position {targetPosition}");
	}

	// Used for testing on button to simulate level complete
	public void UnlockCurrentNode()
	{
		if (currentNode != null && currentNode.isLevelNode)
		{
			currentNode.MarkLevelCompleted();
			//Debug.Log($"Node at position {currentNode.transform.position} is now unlocked and marked as completed.");
		}
		else
		{
			//Debug.Log("Current node is either null or not a level node.");
		}
	}

	private void OnEnable()
	{
		moveAction.Enable();
		switchMovementAction.Enable();
	}
	private void OnDisable()
	{
		moveAction.Disable();
		switchMovementAction.Disable();
	}

	private void EnableDisableShipMovement()
	{

		if (inWater)
		{
			ReattachToGrid(); // Reattach to grid
		}
		else
		{
			DetachFromGrid(); // Detach from grid
		}
	}

	private void DetachFromGrid()
	{
		inWater = true;
		currentSprite.sprite = shipSprite;
		dockUIPanel.SetActive(false);
		// Set the ship's initial position near the dock node
		transform.position = shipTransform.transform.position; // A location will be placed next to dock to begin ship movement
		circleCollider.enabled = true;
		Debug.Log("Detached from grid, controlling the ship.");
	}

	private void ReattachToGrid()
	{
		inWater = false;
		currentSprite.sprite = playerSprite;
		dockUIPanel.SetActive(false);
		// Set the player's position back to the dock node
		transform.position = currentNode.transform.position;
		circleCollider.enabled = false;
		Debug.Log("Reattached to grid at the dock node.");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (inWater && collision.gameObject.CompareTag("Dock"))
		{
			dockUIPanel.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (inWater && collision.gameObject.CompareTag("Dock"))
		{
			dockUIPanel.SetActive(false);
		}
	}
}
