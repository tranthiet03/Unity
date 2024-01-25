using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private float maxHealth, E_speed, knockbackDuration;
    [SerializeField] private Vector2 KnockbackSpeed;

    private int playerFacingDirection;
    private int facingDirection;

    private float currentHealth, knockbackStart;

    [SerializeField] LayerMask whatIsGround;
    private bool groundDetected, wallDetected;
    private bool knockback;

    [SerializeField] private Transform groundCheck, wallCheck;
    [SerializeField] GameObject hitParticles;
    private GameObject Alive;
    private Rigidbody2D rbAlive;
    private Animator aliveAnim;

    private float currentPos;

    private void Start()
    {
        currentHealth = maxHealth;

        Alive = transform.Find("Alive").gameObject;
        rbAlive = Alive.GetComponent<Rigidbody2D>();
        aliveAnim = Alive.GetComponent<Animator>();
        facingDirection = 1;
    }
    private void Update()
    {

        CheckWalkingEnemies();

        CheckKnockback();
    }

    public void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        Instantiate(hitParticles, Alive.transform.position, Alive.transform.rotation);
        if (attackDetails[1] > Alive.transform.position.x)
        {
            playerFacingDirection = -1;
        }
        else
        {
            playerFacingDirection = 1;
        }
        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX("Enemy_Dead");
            FindObjectOfType<GameManager>().AddScore(100);
            
            Destroy(gameObject);
        }
        if (currentHealth > 0)
        {
            AudioManager.instance.PlaySFX("Enemy_Damage");
            Knockback();
        }
    }
    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(KnockbackSpeed.x * playerFacingDirection, KnockbackSpeed.y);
        aliveAnim.SetBool("knockback", knockback);
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            aliveAnim.SetBool("knockback", knockback);
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);

        }
    }
    private void CheckWalkingEnemies()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, 0.1f, whatIsGround);

        if(!knockback)
        {
            rbAlive.velocity = new Vector2(E_speed * facingDirection, rbAlive.velocity.y);
            currentPos = transform.position.x;
        }
        
        if (!groundDetected || wallDetected)
        {
            Flip();
        }

    }
    private void Flip()
    {
        facingDirection *= -1;
        Alive.transform.Rotate(0.0f, 180.0f, 0.0f);

    }
    public float GetCurrentPos()
    {
        return currentPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - 0.5f));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + 0.1f, wallCheck.position.y));
    }
}
