using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathnode : MonoBehaviour
{
	public List<Pathnode> connections = new List<Pathnode>();
	public List<Pathnode> activeConnections = new List<Pathnode>();
	public bool locked = true;

	public bool isLevelNode = false;
	public bool isDockNode = false;
	public bool isLevelCompleted = false;
	public Sprite incompleteLevelSprite; // Need to change over to animated tile/sprite
	public Sprite completeLevelSprite; // Need to change over to animated tile/sprite
	private SpriteRenderer spriteRenderer;


	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateSprite();
	}


	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.yellow;
	//	Gizmos.DrawSphere(transform.position, 0.15f);

	//	foreach (Pathnode target in connections)
	//	{
	//		Gizmos.color = locked ? Color.red : Color.green;
	//		Gizmos.DrawLine(transform.position, target.transform.position);
	//	}
	//}

	public void AddConnection(Pathnode target)
	{
		if (!connections.Contains(target))
		{
			connections.Add(target);
			target.connections.Add(this); // Ensure bidirectional connection
		}
	}

	public void ClearConnections()
	{
		connections.Clear();
	}

	public void UnlockNode()
	{
		locked = false;
		UpdateActiveConnections();
	}

	public void LockNode()
	{
		locked = true;
		UpdateActiveConnections();
	}

	private void UpdateActiveConnections()
	{
		activeConnections.Clear();
		foreach (Pathnode node in connections)
		{
			if (!node.locked)
			{
				activeConnections.Add(node);
			}
		}
	}

	public void MarkLevelCompleted()
	{
		isLevelCompleted = true;
		UnlockNode();
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		if (isLevelNode)
		{
			spriteRenderer.sprite = isLevelCompleted ? completeLevelSprite : incompleteLevelSprite;
		}
	}
}
