using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCombatController : MonoBehaviour
{
    [SerializeField] private int enemyDamage = 5;
    [SerializeField] private LayerMask whatIsPlayer;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float cooldownAttack;
    [SerializeField] private BoxCollider2D boxCollider;
    private Animator anim;

    private Health playerHealth;
    private float PositionEnemy;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if(playerInSight())
        {
            if (cooldownTimer > cooldownAttack)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        GetPositionEnemy();
    }
    private bool playerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,Vector2.left, 0, whatIsPlayer);
        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if(playerInSight())
        {
            playerHealth.TakeDamage(enemyDamage);
            PositionEnemy = transform.position.x;
        }
    }
    public float GetPositionEnemy()
    {
        return PositionEnemy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
}
