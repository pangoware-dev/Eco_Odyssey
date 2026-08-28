// using UnityEngine;

// public class scrDealDamage : MonoBehaviour
// {
//     public int damage;
//     public Transform attackPoint;
//     public float weaponRange;
//     public float knockbackForce=10f;
//     public LayerMask playerLayer;
//     public float stunTime=0.5f;
//     private scrEnemyEco ecoComp;

//     public void Start()
//     {
//         ecoComp = GetComponent<scrEnemyEco>();

//         if (ecoComp != null && ecoComp.ecoData != null)
//         {
//             damage = (int)ecoComp.ecoData.Ataque;
//         }
//     }

//     public void Attack()
//     {
//         Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

//         if(hits.Length > 0)
//         {
//             hits[0].GetComponent<scrLife>().ChangeHealth(damage);
//             hits[0].GetComponent<scrPlayer>().Knockback(transform, knockbackForce, stunTime);
//         }
//     }
// }