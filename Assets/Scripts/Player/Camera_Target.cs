using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Target : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    [SerializeField] private float threshold;
    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = (mousePos - player.position);
		if (direction.magnitude > threshold)
		{
			direction = direction.normalized * threshold;
		}
		transform.position = player.position + direction;
	}
}
