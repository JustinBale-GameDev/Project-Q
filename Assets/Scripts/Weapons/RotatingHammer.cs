using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotatingHammer : MonoBehaviour
{
    public float rotationSpeed;
	public float selfRotationSpeed;
	public int damage;
    public float activeDuration = 5f;
    public float cooldownDuration = 5f;
    public float scaleMultiplyer = 1.1f;
    public float speedMultiplyer = 1.1f;
	public GameObject damageNumberPrefab;

	private bool isActive = false;
    private float activeTime = 0f;
    private float cooldownTime = 0f;
    private Collider2D hammerCollider;
    private SpriteRenderer spriteRenderer;



    void Start()
    {
        hammerCollider = GetComponent<Collider2D>();
        hammerCollider.enabled = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        ActivateHammer(); // Enable weapon right away
    }

    // Update is called once per frame
    void Update()
    {
		if (isActive)
        {
            //RotateHammer();
            RotateHammerAroundPlayer();
            activeTime += Time.deltaTime;

            if (activeTime >= activeDuration)
            {
                DeactivateHammer();
            }
        }
        else
        {
            cooldownTime += Time.deltaTime;

            if (cooldownTime >= cooldownDuration)
            {
                ActivateHammer();
            }
        }
    }

    public void RotateHammerAroundPlayer()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void RotateHammer()
    {
		transform.Rotate(Vector3.forward, selfRotationSpeed * Time.deltaTime);
	}

	public void ActivateHammer()
    {
        isActive = true;
        activeTime = 0f;
        hammerCollider.enabled = true;
		spriteRenderer.enabled = true;

	}

    public void DeactivateHammer()
    {
        isActive = false;
        cooldownTime = 0f;
        hammerCollider.enabled = false;
		spriteRenderer.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy_Health enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ReduceHealth(damage);

				// Use the object pool to spawn a damage number
				ObjectPool_DamageNumber.Instance.SpawnDamageNumber(collision.transform.position, Player_Stats.Instance.damage);
			}
        }
	}

	// Methods for upgrading the hammer
	public void IncreaseDamage(int amount)
	{
		damage += amount;
	}

	public void IncreaseDuration(float amount)
	{
		activeDuration += amount;
	}

	public void IncreaseScale(float amount)
	{
		transform.localScale *= amount;
	}

	public void IncreaseSpeed(float amount)
	{
		rotationSpeed *= amount;
	}
}
