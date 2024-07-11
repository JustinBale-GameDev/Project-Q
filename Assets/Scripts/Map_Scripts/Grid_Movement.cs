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
	private Vector2 moveValue;

	public Pathnode currentNode;
	private Pathnode previousNode;
	public float moveSpeed = 5f;
	private bool isMoving = false;
	private Vector3 targetPosition;

	private void Awake()
	{
		moveAction = inputActions.FindActionMap("GridControls").FindAction("Grid_Movement");
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
	}

	private void FixedUpdate()
	{
		moveValue = moveAction.ReadValue<Vector2>();
	}

	void Update()
	{
		//if (!isMoving)
		//{
		//	if (Input.GetKeyDown(KeyCode.UpArrow))
		//	{
		//		TryMoveToNode(GetConnectedNode(Vector2.up), Vector2.up);
		//	}
		//	else if (Input.GetKeyDown(KeyCode.DownArrow))
		//	{
		//		TryMoveToNode(GetConnectedNode(Vector2.down), Vector2.down);
		//	}
		//	else if (Input.GetKeyDown(KeyCode.LeftArrow))
		//	{
		//		TryMoveToNode(GetConnectedNode(Vector2.left), Vector2.left);
		//	}
		//	else if (Input.GetKeyDown(KeyCode.RightArrow))
		//	{
		//		TryMoveToNode(GetConnectedNode(Vector2.right), Vector2.right);
		//	}
		//}

		if (!isMoving && moveValue != Vector2.zero)
		{
			Vector2 direction = moveValue.normalized;
			Debug.Log(direction.ToString());
			Pathnode targetNode = GetConnectedNode(direction);
			TryMoveToNode(targetNode, direction);
		}

		if (isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				isMoving = false;
				transform.position = targetPosition;
				Debug.Log("Reached Target Node");
			}
		}
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
		Debug.Log("No connected node found in the given direction.");
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
				Debug.Log("Cannot move forward past a locked node.");
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
		isMoving = true;
		Debug.Log($"Moving to node at position {targetPosition}");
	}

	// Used for testing on button to simulate level complete
	public void UnlockCurrentNode()
	{
		if (currentNode != null && currentNode.isLevelNode)
		{
			currentNode.MarkLevelCompleted();
			Debug.Log($"Node at position {currentNode.transform.position} is now unlocked and marked as completed.");
		}
		else
		{
			Debug.Log("Current node is either null or not a level node.");
		}
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
