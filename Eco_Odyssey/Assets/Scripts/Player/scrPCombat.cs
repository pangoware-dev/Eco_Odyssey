// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// public class scrPCombat : MonoBehaviour
// {

//     public Transform attackPoint;
//     public float weaponRange = 0.4f;
//     public float fakeRange = 1f;
//     public LayerMask enemyLayer;
//     public int Ataque;
//     public float knockbackForce = 10f;
//     public float stunTime = 0.5f;

//     public Animator anim;

//     public float cooldown = 1f;
//     private float timer;


//     private void Update()
//     {
//         if(timer > 0)
//         {
//             timer -= Time.deltaTime;
//         }

//         EnemyDodge();
//     }


//     public void Attack()
//     {
//         if (timer <= 0)
//         {
//             anim.SetBool("isAttacking", true);
//             timer = cooldown;
//         }
//     }

//     public void DealDamage()
//     {
//             Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

//             if(enemies.Length > 0)
//             {
//                 enemies[0].GetComponent<scrEHealth>().changeHP(Ataque);
//                 enemies[0].GetComponent<scrEnemyKB>().Knockback(transform, knockbackForce, stunTime);
//             }
//     }

//     public void EnemyDodge()
//     {
//         Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, fakeRange, enemyLayer);

//         if(enemies.Length > 0 && anim.GetBool("isAttacking") == true)
//         {
//             enemies[0].GetComponent<scrChase>().canDodge(1);
//         }
//     }

//     // public void StopAttack()
//     // {
//     //     anim.SetBool("isAttacking", false);
//     // }

//     //Para visualizar o ataque
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
//         Gizmos.DrawWireSphere(attackPoint.position, fakeRange);
//     }
// }