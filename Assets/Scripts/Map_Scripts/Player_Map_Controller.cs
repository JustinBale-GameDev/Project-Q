using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Map_Controller : MonoBehaviour
{
	private Player_Grid_Movement playerGridMovement;
    private Player_Ship_Movement playerShipMovement;

	public bool onGrid = false;
	public bool inWater = false;

	public GameObject dockUIPanel; // Gets enabled/disabled when on or near dock node
	//public TMP_Text proptBoatTransitionText; // Used on Panel when going 
	//public TMP_Text promptGridTransitionText;

	[SerializeField]
	private InputActionAsset inputActions;
	private InputAction moveAction;
	private InputAction switchMovementAction;
	public Vector2 moveValue;
	public Rigidbody2D rb;

	private int facingDirection = 1;

	public SpriteRenderer currentSprite;
	public Sprite shipSprite;
	public Sprite playerSprite;

	private CircleCollider2D circleCollider; // used for collision when inWater is true

	private void Awake()
	{
		moveAction = inputActions.FindActionMap("GridControls").FindAction("Grid_Movement");
		switchMovementAction = inputActions.FindActionMap("GridControls").FindAction("EnterExitShip");
	}

	void Start()
    {
		playerGridMovement = GetComponent<Player_Grid_Movement>();
		playerShipMovement = GetComponent<Player_Ship_Movement>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();


		currentSprite.sprite = playerSprite;
		dockUIPanel.SetActive(false);
	}

	private void FixedUpdate()
	{
		moveValue = moveAction.ReadValue<Vector2>();

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
		// Change between using playerGridMovement or playerShipMovement scripts depending on if onGrid of inWater
		// Handle switching between movement scripts
		if (onGrid)
		{
			playerGridMovement.enabled = true;
			playerShipMovement.enabled = false;
		}
		else if (inWater)
		{
			playerGridMovement.enabled = false;
			playerShipMovement.enabled = true;
		}
		else
		{
			playerGridMovement.enabled = false;
			playerShipMovement.enabled = false;
		}

		// Check for action to switch to ship or grid
		if (dockUIPanel.activeSelf && switchMovementAction.WasPressedThisFrame())
		{
			EnableDisableShipMovement();
		}
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

		// This line needs to get the ship transform from the dock node. Each dock node should have a reference to a ship location. Each docks ship location will be different. When switching, the players transform will becomes this ship transform.
		//transform.position = shipTransform.transform.position;


		circleCollider.enabled = true; // Enabled collider so ship can interact with land and stay on water
		Debug.Log("Detached from grid, controlling the ship.");
	}

	private void ReattachToGrid()
	{
		inWater = false;
		currentSprite.sprite = playerSprite;
		dockUIPanel.SetActive(false);
		
		// This line needs to find the node that allowed the boat to grid switch and become the current node as well as change the players transform to current node.
		//transform.position = currentNode.transform.position;


		circleCollider.enabled = false; // Disabeld to allow for grid movement
		Debug.Log("Reattached to grid at the dock node.");
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
}
