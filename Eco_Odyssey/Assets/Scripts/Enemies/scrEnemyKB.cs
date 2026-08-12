using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scrEnemyKB : MonoBehaviour
{
    private Rigidbody2D rb;
    private scrChase chaseScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chaseScript = GetComponent<scrChase>();
    }
    public void Knockback(Transform playerTransform, float knockbackForce, float stunTime)
    {
        chaseScript.ChangeState(EnemyState.Knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector2 direction=(transform.position-playerTransform.position).normalized;
        rb.linearVelocity=direction*knockbackForce;
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity=Vector2.zero;
        chaseScript.ChangeState(EnemyState.Idle);
    }
}