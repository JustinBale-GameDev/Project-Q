using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ship_Movement : MonoBehaviour
{
	public Player_Map_Controller playerMapController;
	private Player_Grid_Movement playerGridMovement;

	public int shipMoveSpeed = 3;

	

	// Start is called before the first frame update
	void Start()
    {
		playerMapController = GetComponent<Player_Map_Controller>();
		playerGridMovement = GetComponent<Player_Grid_Movement>();
	}

	private void FixedUpdate()
    {
		// Top down movement
		if (playerMapController.inWater)
		{
			playerMapController.rb.MovePosition(playerMapController.rb.position + shipMoveSpeed * Time.deltaTime * playerMapController.moveValue);
		}

		//playerMapController.rb.MovePosition(playerMapController.rb.position + shipMoveSpeed * Time.deltaTime * playerMapController.moveValue);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Dock"))
		{
			playerMapController.dockUIPanel.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Dock"))
		{
			playerMapController.dockUIPanel.SetActive(false);
		}
	}
}
