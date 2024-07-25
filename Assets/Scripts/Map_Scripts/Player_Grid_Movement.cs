using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Grid_Movement : MonoBehaviour
{
    public Player_Map_Controller playerMapController;
	private Player_Ship_Movement playerShipMovement;

	public Pathnode currentNode;
	private Pathnode previousNode;
	private Vector3 targetPosition;
	private bool isMovingToNode = false;

	public int gridMoveSpeed = 7;
    // Start is called before the first frame update
    void Start()
    {
		playerMapController = GetComponent<Player_Map_Controller>();
		playerShipMovement = GetComponent<Player_Ship_Movement>();

		if (currentNode != null)
		{
			transform.position = currentNode.transform.position;
		}
		else
		{
			Debug.LogError("Current node is not assigned.");
		}
	}

    // Update is called once per frame
    void Update()
    {
		// If player is not currently moving to a node, input is detected, and not in water
		if (!isMovingToNode && playerMapController.moveValue != Vector2.zero)
		{
			Vector2 direction = playerMapController.moveValue.normalized;
			Pathnode targetNode = GetConnectedNode(direction);
			TryMoveToNode(targetNode, direction);
		}

		if (isMovingToNode)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, gridMoveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				isMovingToNode = false;
				transform.position = targetPosition;
				//Debug.Log("Reached Target Node");

				// Enable UI for dock node and allow for input to switch to ship
				if (currentNode.isDockNode)
				{
					playerMapController.dockUIPanel.SetActive(true);
				}
				else
				{
					playerMapController.dockUIPanel.SetActive(false);
				}
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
}
