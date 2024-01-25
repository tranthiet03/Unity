using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombatController : MonoBehaviour
{
    //isAttack cản các Anim khác ảnh hưởng vào Anim tấn công
    //firstAttack chuyển luôn phiên 2 anim Attack
    //canAttack dừng attack
    //attack1 == True, khi tấn công
    //gotInput xác nhận đã nhận đầu vào chưa
    [SerializeField] 
    private bool combatEnabled;
    private bool gotInput, isAttacking, isFirstAttack;

    [SerializeField] GameObject hitParticles;

    [SerializeField] float inputTimer, attack1Damage = 5f;

    private float lastInputTime = Mathf.NegativeInfinity;//lưu trữ lần cuối tấn công;

    [SerializeField] private Transform attack1HitBoxPos;
    [SerializeField] private LayerMask whatIsDamageable;

    private Animator anim;
    private float[] attackDetails = new float[2];
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);//cho phép đánh nhau khi bắt đầu game
    }
    void Update()
    {
        CheckCombat();
        CheckAttacks();
    }

    private void CheckCombat() //Xác định đầu vào Input attack
    {
        if (Input.GetKey(KeyCode.J))
        {
            if(combatEnabled)
            {
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }
    private void CheckAttacks()//thực hiện tấn công khi nhận được đầu vào   
    {
        if (gotInput)
        {
            //thực hiện tấn công
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack; //được dùng để chuyển luân phiên 2 Anim Attack
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
                AudioManager.instance.PlaySFX("Player_Sword");
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, 0.8f, whatIsDamageable);

        attackDetails[0] = attack1Damage;
        attackDetails[1] = transform.position.x;



        foreach (Collider2D collider in detectedObjects)
        {
            EnemiesController enemyController = collider.transform.parent.GetComponentInChildren<EnemiesController>();
            if (enemyController != null)
            {
                enemyController.Damage(attackDetails);
            }
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, 0.8f);
    }
}
