using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private PlayerController PC;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] GameObject hitParticles;
    private void Awake()
    {
        PC = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    public void TakeDamage(int _damage)
    {
        Instantiate(hitParticles,transform.position,transform.rotation);
        currentHealth -= _damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth > 0)
        {
            AudioManager.instance.PlaySFX("Player_Damage");
            PC.Knockback();
        }
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }
    private void Die()
    {
        AudioManager.instance.PlaySFX("Player_Dead");
        AudioManager.instance.musicSource.Stop();
        FindObjectOfType<PauseGame>().GameOver();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            currentHealth = 0;
            healthBar.SetHealth(currentHealth);
            Die();
        }
    }
}
