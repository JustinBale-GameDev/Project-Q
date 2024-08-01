using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float duration = 1.0f;

    private float timer;
    private TextMesh textMesh;

	void Awake()
	{
		textMesh = GetComponent<TextMesh>();
	}

	void Update()
    {
        // Move the damage number upwards
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Check if the damage number should disappear
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
		// Set the text to dispaly the damage amount
		textMesh.text = damage.ToString();
    }
}
